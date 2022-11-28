window.addEventListener("load", () => {
    const maxImg = 53;

    let currentImg = 1;

    const leftArrow  = document.getElementById("prev-img");
    const rightArrow = document.getElementById("next-img");

    const imgPrev = document.getElementById("img-frame-prev");
    const img = document.getElementById("img-frame");
    const imgNext = document.getElementById("img-frame-next");

    leftArrow.addEventListener("click", () => {
        currentImg--;
        if (currentImg < 0)currentImg = maxImg;

        imgPrev.src = `/img/card_${currentImg - 1 >= 0 ? currentImg - 1 : maxImg}.png`;
        img.src = `/img/card_${ currentImg }.png`;
        imgNext.src = `/img/card_${currentImg + 1 <= maxImg ? currentImg + 1 : 0}.png`;
    });

    rightArrow.addEventListener("click", () => {
        currentImg++;
        if (currentImg >maxImg)currentImg = 0;

        imgPrev.src = `/img/card_${currentImg - 1 >= 0 ? currentImg - 1 : maxImg}.png`;
        img.src = `/img/card_${ currentImg }.png`;
        imgNext.src = `/img/card_${currentImg + 1 <= maxImg ? currentImg + 1 : 0}.png`;
    });
});