document.addEventListener("DOMContentLoaded", function () {
    const countrySelect = document.getElementById('country'),
        form = document.forms[0],
        submitBtn = form.querySelector('button[type="submit"]'),
        resetBtn = form.querySelector('button[type="reset"]'),
        categorySelect = document.getElementById('category');

    //------ Загрузка городов
    countrySelect.addEventListener("change", function () {
        if (this.value) {
            loadCities(this.value, citySelect);
        } else {
            resetCities(citySelect);
        }
    });
    
    //-------- Загрузка фото
    handleImageUpload(inputElement, container, namePlace);

    //-------- Кнопка "Отмена"
    setupCancelButton(cancelBtn);

    //-------- Кнопка "Очистить"
    resetBtn.addEventListener('click', (event) => {
        event.preventDefault();
        openModal('Вы уверены, что хотите очистить форму?', () => {
            container.innerHTML = '';
            resetCities(citySelect);
            form.reset();
        }, modalWindow, modalWindowBox);
    });

    // Проверка и отправка
    submitBtn.addEventListener('click', (event) => {
        event.preventDefault();
        const messages = [];

        if (countrySelect.value && !citySelect.value) {
            messages.push('Выберите город. Страна без выбранного города не сохранится.');
        }
        if (!categorySelect.value) {
            messages.push('Выберите категорию. Если категория не будет выбрана, место получит категорию "Другое".');
        }

        if (messages.length > 0) {
            showNextModal(messages, 0);
            return;
        }
        form.submit();
    });

    function showNextModal(messages, index) {
        if (index < messages.length) {
            openModal(messages[index], () => {
                showNextModal(messages, index + 1);
            }, modalWindow, modalWindowBox);
        } else {
            form.submit();
        }
    }
});