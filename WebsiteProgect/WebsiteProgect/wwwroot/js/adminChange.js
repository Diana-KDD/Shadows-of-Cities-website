document.addEventListener("DOMContentLoaded", function () {
    const countrySelect = document.getElementById('country');
        

    const selectedCityIdValue = selectedCityId ? selectedCityId.value : null;

    //----- Показываем существующие фото
    if (typeof existingImages !== 'undefined') {
        renderExistingImages(container, existingImages);
    }

    //----- Загрузка фото
    handleImageUpload(inputElement, container, namePlace);

    //---- Загрузка городов при смене страны
    countrySelect.addEventListener('change', function () {
        loadCities(this.value, citySelect, selectedCityIdValue);
    });

    //----- Загрузка городов сразу, если страна выбрана
    if (countrySelect.value) {
        loadCities(countrySelect.value, citySelect, selectedCityIdValue);
    }

    //----- Кнопка "Отмена"
    setupCancelButton(cancelBtn);
});