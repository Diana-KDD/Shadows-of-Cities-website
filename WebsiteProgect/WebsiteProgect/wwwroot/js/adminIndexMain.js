const modalWindow = document.querySelector('.modalUpdate'),
modalWindowBox = document.querySelector('.modalUpdate .box'),
btnCategoryChange = document.querySelector('.categoryChange'),
btnCountryChange = document.querySelector('.countryChange'),
btnCityChange = document.querySelector('.cityChange');

//--------------------------------------------------------------

btnCategoryChange.addEventListener('click', async () => {
    modalWindow.style.display = 'flex';
    await renderModalWindowForm('Category', 'LoadList');
});

btnCountryChange.addEventListener('click', async () => {
    modalWindow.style.display = 'flex';
    await renderModalWindowForm('Country', 'LoadList');
});

btnCityChange.addEventListener('click', async () => {
    modalWindow.style.display = 'flex';
    await renderModalWindowForm('City', 'LoadList');
});

//--------------------------------------------------------------

async function renderModalWindowForm(dataType, actionType, data = null) {

    while (modalWindowBox.firstChild) {
        modalWindowBox.removeChild(modalWindowBox.firstChild);
    }

    if (actionType === 'CloseModal') {
        modalWindow.style.display = 'none';
        return;
    }

    const btnClose = createButton(' X ', () => renderModalWindowForm(dataType, 'CloseModal'));
    modalWindowBox.appendChild(btnClose);

    const title = document.createElement('h2');
    title.textContent = DATA_TYPES[dataType].title;
    modalWindowBox.appendChild(title);

    if (actionType === 'LoadList') await loadListAndActions(dataType);
    else if (actionType === 'Delete') confirmationDeletion(dataType, data);
    else formAddAndChange(dataType, actionType, data);
}

//------------ Загрузка списка существующих данных из бд
async function loadListAndActions(dataType) {
    try {

        const response = await fetch(DATA_TYPES[dataType].api.list);
        const data = await response.json();

        if (data.length === 0) {
            const title2 = document.createElement('h3');
            title2.textContent = 'Данных нет. Добавьте.';
            modalWindowBox.appendChild(title2);
        }
        else {
            data.forEach(item => {
                const nameCard = document.createElement('h3');
                nameCard.textContent = item.name;
                const btnChangeCard = createButton('Изменить', () => renderModalWindowForm(dataType, 'Change', item));
                const btnDeleteCard = createButton('Удалить', () => renderModalWindowForm(dataType, 'Delete', item));

                const div = document.createElement('div');
                div.className = 'cardModalWindow';
                div.appendChild(nameCard);
                div.appendChild(btnChangeCard);
                div.appendChild(btnDeleteCard);

                if (DATA_TYPES[dataType].hasParent) {
                    let divParent = modalWindowBox.querySelector(`.${DATA_TYPES[dataType].parentType}-${item.countryId}`)
                    if (!divParent) {
                        const nameParent = document.createElement('h3');
                        nameParent.className = DATA_TYPES[dataType].parentType;
                        nameParent.textContent = item.countryName;;

                        divParent = document.createElement('div');
                        divParent.className = `listCity ${DATA_TYPES[dataType].parentType}-${item.countryId}`

                        divParent.appendChild(nameParent);
                    }
                    divParent.appendChild(div);
                    modalWindowBox.appendChild(divParent);
                }
                else {
                    modalWindowBox.appendChild(div);
                }   
            })
        }
        const btnAddCard = createButton(' + ', () => renderModalWindowForm(dataType, 'Add'));
        modalWindowBox.appendChild(btnAddCard);
    }
    catch (error) {
        console.error('Ошибка загрузки данных:', error);
        alert('Не удалось загрузить данные');
    }

}

//--------------- Создание формы для создания/изменения
async function formAddAndChange(dataType, actionType, data) {

    const h3 = document.createElement('h3');
    if (actionType === 'Add') {
        h3.textContent = DATA_TYPES[dataType].newTitle;
    }
    else {
        h3.textContent = DATA_TYPES[dataType].editTitle(data.name);
    }
    modalWindowBox.appendChild(h3);

    const form = document.createElement('form');

    const label = document.createElement('label');
    label.textContent = 'Название:';
    label.for = 'name';

    const input = document.createElement('input');
    input.id = 'name';
    input.name = 'Name';
    input.type = 'text';
    input.required = true;
    input.minLength = 2;
    input.maxLength = 50;
    input.placeholder = `Например: ${DATA_TYPES[dataType].placeholder}`;

    if (actionType === 'Change') input.value = `${data.name}`;
    
    if (DATA_TYPES[dataType].hasParent) {
        const labelParent = document.createElement('label');
        labelParent.textContent = DATA_TYPES[dataType].parentLabel;
        labelParent.for = 'selectParent';

        const select = document.createElement('select');
        select.id = 'selectParent';
        select.addEventListener("change", async function () {
            const inputName = modalWindowBox.querySelector('input');
            if (this.value != '') {
                inputName.disabled = false;
            }
            else {
                inputName.disabled = true;
            }
        })

        if (actionType === 'Add') {
            input.disabled = true;
            const emptyOption = document.createElement('option');
            emptyOption.value = '';
            emptyOption.textContent = 'Выберите';
            emptyOption.selected = true;
            select.insertBefore(emptyOption, select.firstChild);
        }

        const response = await fetch(DATA_TYPES[dataType].parentApi);
        const dataResponse= await response.json();

        dataResponse.forEach(item => {
            const option = document.createElement('option');
            option.value = item.id;
            option.textContent = item.name;
            if (actionType === 'Change') {
                if (item.name === data.countryName) {
                    option.selected = true;
                }
            }
            select.appendChild(option);
        });

        form.appendChild(labelParent);
        form.appendChild(select);
    }

    form.appendChild(label);
    form.appendChild(input);

    modalWindowBox.appendChild(form);

    const div = document.createElement('div');
    div.className = 'btnsAction';

    const btnNOSave = createButton('Отмена', () => renderModalWindowForm(dataType, 'LoadList'));
    div.appendChild(btnNOSave);

    let btnSave;
    if (actionType === 'Add') btnSave = createButton('Сохранить', async () => submitAddOrChange(dataType, 'Add'));
    else btnSave = createButton('Сохранить', async () => submitAddOrChange(dataType,'Change', data.id));
    div.appendChild(btnSave);

    modalWindowBox.appendChild(div);
}

//------------- Подтверждение удаления
function confirmationDeletion(dataType, data) {

    const p = document.createElement('p');
    p.innerHTML = DATA_TYPES[dataType].deleteWarning(data.name)
    modalWindowBox.appendChild(p);

    const div = document.createElement('div');
    div.className = 'btnsAction';

    const btnNODelete = createButton('Отмена', () => renderModalWindowForm(dataType, 'LoadList'));
    div.appendChild(btnNODelete);

    const btnDelete = createButton('Удалить', async () => { await submitdelete(dataType, data.id) });
    div.appendChild(btnDelete);

    modalWindowBox.appendChild(div);
}

//---------------- Отправка запроса на создание/изменение
async function submitAddOrChange(dataType, actionType, id = null) {

    const input = document.getElementById('name');
    const name = input.value.trim();
    let parentId;

    if (DATA_TYPES[dataType].hasParent) {
        const select = document.getElementById('selectParent');
        parentId = select.value;

        if (!parentId) {
            alert('Выберите элемент списка');
            return;
        }
    }

    if (!name || name.length < 2) {

        alert('Название должно быть не менее 2 символов.');
        return;
    }

    buttonLock();

    try {
        const method = actionType === 'Add' ? 'POST' : 'PUT';

        const response = await fetch((actionType === 'Add' ? DATA_TYPES[dataType].api.add : DATA_TYPES[dataType].api.change(id)), {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: DATA_TYPES[dataType].body(name, parentId)
        });

        await handleApiResponse(dataType, response);

    } catch (error) {
        console.error('Ошибка:', error);
        alert(`Не удалось ${actionType === 'Add' ? 'сохранить' : 'изменить'} данные: ${error.message}`);
    } finally {
        buttonUnlock();
    }
}

//---------------- Отправка запроса на удаление
async function submitdelete(dataType, id = null) {

    if (id === null) {
        alert('Ошибка удаления');
        return;
    }
    buttonLock();
    try {
        const response = await fetch(DATA_TYPES[dataType].api.delete(id), {
            method: 'DELETE'
        });
        await handleApiResponse(dataType, response);
    }
    catch (error) {
        console.error('Ошибка:', error);
        alert('Не удалось удалить.');
    }
    finally {
        buttonUnlock();
    }
}

//-------- Блокировка кнопок
function buttonLock() {
    const btnsModal = modalWindowBox.querySelectorAll('.btnModal');
    btnsModal.forEach(
        btn => {
            btn.disabled = true;
        }
    )
}

//--------- Разблокировка кнопок
function buttonUnlock() {
    const btnsModal = modalWindowBox.querySelectorAll('.btnModal');
    btnsModal.forEach(
        btn => {
            btn.disabled = false;
        }
    )
}

//---------- Обработка ответа
async function handleApiResponse(dataType, response) {
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Ошибка операции');
    }
    await renderModalWindowForm(dataType, 'LoadList');
}

//------------ Функция создания кнопок
function createButton(text, onClick) {
    const btn = document.createElement('button');
    btn.textContent = text;
    btn.className = 'btnModal';
    btn.addEventListener('click', onClick);
    return btn;
}

//-------- Функция экранирования
function escapeHtml(unsafe) {
    return unsafe.replace(/[&<>"']/g, function (m) {
        return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#039;' }[m];
    });
}