// ========== ОБЩИЕ ФУНКЦИИ ДЛЯ СОЗДАНИЯ И РЕДАКТИРОВАНИЯ ==========

//--- Загружает города для выбранной страны ---
const citySelect = document.getElementById('city'),        //- элемент <select> для городов
      selectedCityId = document.getElementById('cityId');  //- ID города, который нужно выбрать(для редактирования)


async function loadCities(countryId, citySelect, selectedCityId = null) {
    citySelect.innerHTML = '<option value="">Загрузка...</option>';
    citySelect.disabled = true;

    if (!countryId) {
        citySelect.innerHTML = '<option value="">-- Сначала выберите страну --</option>';
        return;
    }

    try {
        const response = await fetch(`/api/Api/GetCitiesByCountry?countryId=${countryId}`);
        if (!response.ok) throw new Error(`Ошибка сервера: ${response.status}`);

        const cities = await response.json();

        citySelect.innerHTML = '<option value="">-- Выберите город --</option>';
        citySelect.disabled = false;

        cities.forEach(c => {
            const option = document.createElement("option");
            option.value = c.id;
            option.textContent = c.name;
            if (selectedCityId && c.id == selectedCityId) {
                option.selected = true;
            }
            citySelect.appendChild(option);
        });
    } catch (error) {
        console.error("Ошибка загрузки городов:", error);
        citySelect.innerHTML = '<option value="">Ошибка загрузки</option>';
    }
}

//------ Сбрасывает выпадающий список городов
function resetCities(citySelect) {
    citySelect.innerHTML = '<option value="">-- Сначала выберите страну --</option>';
    citySelect.disabled = true;
}

//--- Обработчик загрузки и проверки фото ---
const inputElement = document.getElementById('image'),          //- input файлов
      container = document.querySelector('.imageContainer'),    //- контейнер для картинок
      namePlace = document.getElementById('name');              //- название места

function handleImageUpload(inputElement, container, namePlace) {
    const maxFiles = 7,
        maxSize = 5 * 1024 * 1024,
        allowedTypes = ['image/jpeg', 'image/png', 'image/webp'];

    inputElement.addEventListener('change', function () {
        let errorMessage = '';
        container.innerHTML = '';

        if (this.files.length > maxFiles) {
            errorMessage = `Можно загрузить не более ${maxFiles} фото.`;
        }

        if (!errorMessage) {
            const invalidFiles = [];
            const oversizedFiles = [];

            for (const file of this.files) {
                if (!allowedTypes.includes(file.type)) {
                    invalidFiles.push(file.name);
                }
                if (file.size > maxSize) {
                    oversizedFiles.push(file.name);
                }
            }

            if (invalidFiles.length > 0) {
                errorMessage = `Неподдерживаемый формат:\n${invalidFiles.join('\n')}`;
            }
            if (oversizedFiles.length > 0) {
                errorMessage = `Файлы превышают ${Math.round(maxSize / 1024 / 1024)} МБ:\n${oversizedFiles.join('\n')}`;
            }
        }

        if (errorMessage) {
            alert(errorMessage);
            this.value = '';
        } else {
            for (const file of this.files) {
                const img = document.createElement('img');
                img.src = URL.createObjectURL(file);
                img.alt = file.name ? file.name : `Фото ${namePlace}`;
                container.appendChild(img);
            }
        }
    });
}

//------ Модальное окно ---
const modalWindow = document.querySelector('.modalWindow'),
      modalWindowBox = modalWindow.querySelector('.box');

//------- Создаёт кнопку для модального окна
function createModalButton(text, onClick) {
    const btn = document.createElement('button');
    btn.textContent = text;
    btn.className = 'btnModal';
    btn.addEventListener('click', onClick);
    return btn;
}

//-------- Открывает модальное окно с сообщением и действиями
function openModal(text, onContinue, modalWindow, modalWindowBox) {
    while (modalWindowBox.firstChild) {
        modalWindowBox.removeChild(modalWindowBox.firstChild);
    }
    modalWindow.style.display = 'flex';

    const p = document.createElement('p');
    p.textContent = text;

    const divBtns = document.createElement('div');
    divBtns.className = 'btnsAction';

    const btnNo = createModalButton('Отмена', () => closeModal(modalWindow));
    const btnOk = createModalButton('Продолжить', () => {
        closeModal(modalWindow);
        if (onContinue) onContinue();
    });

    modalWindowBox.appendChild(p);
    divBtns.appendChild(btnNo);
    divBtns.appendChild(btnOk);
    modalWindowBox.appendChild(divBtns);
}

//------- Закрывает модальное окно
function closeModal(modalWindow) {
    modalWindow.style.display = 'none';
}

//------- Настройка кнопки "Отмена" — возврат в админку ---
const cancelBtn = document.querySelector('.cancel');

function setupCancelButton(button) {
    button.addEventListener('click', () => {
        window.location.href = '/Admin/Index';
    });
}

//------- Отображает существующие фото (для режима редактирования)
function renderExistingImages(container, existingImages) {
    if (!container || !existingImages || existingImages.length === 0) {
        if (container) container.innerHTML = '<p>Фото отсутствуют</p>';
        return;
    }

    container.innerHTML = '';
    existingImages.forEach(image => {
        const img = document.createElement('img');
        img.src = image.path;
        img.alt = image.name;
        container.appendChild(img);
    });
}