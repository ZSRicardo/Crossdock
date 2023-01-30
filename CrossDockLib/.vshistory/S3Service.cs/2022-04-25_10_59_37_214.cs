using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossDockLib
{
    public class S3Service
    {
        private readonly AmazonS3Client _amazonS3;

        // Solo puede ser iniciado su valor en el constructor y la variable ser utilizada en la clase
        private readonly string _bucketName;
        private readonly string _accessKey;
        private readonly string _secretKey;

        private readonly string _urlS3;

        public S3Service()
        {

            // Obtener valores de app.config
            _accessKey = ConfigurationManager.AppSettings["AccessKey"];
            _secretKey = ConfigurationManager.AppSettings["S3SecretKey"];
            _bucketName = ConfigurationManager.AppSettings["S3BucketName"];

            // Inicializar objeto de HoneyCode
            _amazonS3 = new AmazonS3Client(_accessKey, _secretKey, Amazon.RegionEndpoint.USEast1);

            _urlS3 = $"https://{_bucketName}.s3.amazonaws.com/";
        }


        public async Task CrearFolderAsync(string newFolderPath)
        {

            // Checa si existe un folder/path especificado
            var findFolderRequest = new ListObjectsV2Request();
            findFolderRequest.BucketName = _bucketName;
            findFolderRequest.Prefix = newFolderPath;
            findFolderRequest.MaxKeys = 1;

            ListObjectsV2Response findFolderResponse = await _amazonS3.ListObjectsV2Async(findFolderRequest);

            if (findFolderResponse.S3Objects.Any())
            {
                return;
            }

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                StorageClass = S3StorageClass.Standard,
                ServerSideEncryptionMethod = ServerSideEncryptionMethod.None,
                Key = newFolderPath, // <-- in S3 key represents a path  
                ContentBody = string.Empty
            };

            // add try catch in case you have exceptions shield/handling here 
            PutObjectResponse response = await _amazonS3.PutObjectAsync(request);

        }

        public async Task<string> UploadFileToS3(byte[] file, string fileName, string contentType)
        {
            using (var newMemoryStream = new MemoryStream(file))
            {

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = newMemoryStream,
                    Key = fileName,
                    ContentType = contentType,
                    BucketName = _bucketName,
                    CannedACL = S3CannedACL.NoACL
                };

                var fileTransferUtility = new TransferUtility(_amazonS3);

                await fileTransferUtility.UploadAsync(uploadRequest);

                return _urlS3 + fileName;
            }

        }


    }
}
