/* === SWIPER НАСТРОЙКИ === */
const swiper = new Swiper('.swiper', {
    loop: true,
    speed: 500,
    grabCursor: true,

    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },

    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    }
});