// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function customFormWizard(formSteps) {
    // Seleccion de elementos en form
    let step = document.getElementsByClassName('step');
    let prevBtn = document.getElementById('prev-btn');
    let nextBtn = document.getElementById('next-btn');
    let submitBtn = document.getElementById('submit-btn');
    let form = document.getElementById('qbox-form');

    let current_step = 0;
    // Numero de pasos que tiene el form
    let stepCount = formSteps;

    step[current_step].classList.add('d-block');
    if (current_step == 0) {
        prevBtn.classList.add('d-none');
        submitBtn.classList.add('d-none');
        nextBtn.classList.add('d-inline-block');
    }

    // Barra de progreso
    const progress = (value) => {
        document.getElementsByClassName('progress-bar')[0].style.width = `${value}%`;
    }

    nextBtn.addEventListener('click', () => {
        current_step++;
        let previous_step = current_step - 1;
        if ((current_step > 0) && (current_step <= stepCount)) {
            prevBtn.classList.remove('d-none');
            prevBtn.classList.add('d-inline-block');
            step[current_step].classList.remove('d-none');
            step[current_step].classList.add('d-block');
            step[previous_step].classList.remove('d-block');
            step[previous_step].classList.add('d-none');
            if (current_step == stepCount) {
                submitBtn.classList.remove('d-none');
                submitBtn.classList.add('d-inline-block');
                nextBtn.classList.remove('d-inline-block');
                nextBtn.classList.add('d-none');
            }
        } else {
            if (current_step > stepCount) {
                form.onsubmit = () => {
                    return true
                }
            }
        }
        progress((100 / stepCount) * current_step);
    });


    prevBtn.addEventListener('click', () => {
        if (current_step > 0) {
            current_step--;
            let previous_step = current_step + 1;
            prevBtn.classList.add('d-none');
            prevBtn.classList.add('d-inline-block');
            step[current_step].classList.remove('d-none');
            step[current_step].classList.add('d-block')
            step[previous_step].classList.remove('d-block');
            step[previous_step].classList.add('d-none');
            if (current_step < stepCount) {
                submitBtn.classList.remove('d-inline-block');
                submitBtn.classList.add('d-none');
                nextBtn.classList.remove('d-none');
                nextBtn.classList.add('d-inline-block');
                prevBtn.classList.remove('d-none');
                prevBtn.classList.add('d-inline-block');
            }
        }

        if (current_step == 0) {
            prevBtn.classList.remove('d-inline-block');
            prevBtn.classList.add('d-none');
        }
        progress((100 / stepCount) * current_step);
    });


    submitBtn.addEventListener('click', () => {
       
        prevBtn.classList.remove('d-inline-block');
        prevBtn.classList.add('d-none');

        submitBtn.innerText = 'Enviando';
        submitBtn.setAttribute('disabled', true);

        form.submit();

        /* Probar submit con un delay de 4 segundos
        setTimeout(function () {
            form.submit();
        }, 4000);
        
         */

    });
}
