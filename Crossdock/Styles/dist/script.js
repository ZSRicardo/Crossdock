var tl = new TimelineMax({
	delay: 1
});

tl
	.to($(".panel"), 1, { opacity: 1, scale: 1 })
	.staggerFrom($(".panel > .title div"), 0.5, { opacity: 0, x: -50 }, 0.4)
	.staggerFrom($(".items .item"), 0.5, { opacity: 0, x: -15 }, 0.4);