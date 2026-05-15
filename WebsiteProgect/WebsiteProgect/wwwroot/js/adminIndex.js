const modalCategory = document.querySelector('.modalCategory');
modalCategoryBox = document.querySelector('.modalCategory .box');
    btnCategoryChange = document.getElementById('categoryChange');

btnCategoryChange.addEventListener('click', async () => {
    modalCategory.style.display = 'flex';
    await renderCategoryForm('LoadList');
});

async function renderCategoryForm(type, category = null) {
    while (modalCategoryBox.firstChild) {
        modalCategoryBox.removeChild(modalCategoryBox.firstChild);
    }
    if (type === 'CloseModal') {
        modalCategory.style.display = 'none';
        return;
    }

    const btnClose = createButton(' X ', () => renderCategoryForm('CloseModal'));
    modalCategoryBox.appendChild(btnClose);

    const title = document.createElement('h2');
    title.textContent = 'Редактор категорий';
    modalCategoryBox.appendChild(title);

    if (type === 'LoadList') await loadListCategoriesAndActions();
    else if (type === 'Delete') confirmationDeletion(category);
    else formCategories(type, category);
}

//Загрузка списка существующих категорий в бд
async function loadListCategoriesAndActions() {
    
    try {

        const response = await fetch(`/api/Api/GetCategories`);
        const categories = await response.json();

        if (categories.length === 0) {
            const title2 = document.createElement('h3');
            title2.textContent = 'Категорий нет. Добавьте первую.';
            modalCategoryBox.appendChild(title2);
        }
        else {
            categories.forEach(category => {
                const div = document.createElement('div');
                div.className = 'cardCategory';

                const nameCategory = document.createElement('h3');
                nameCategory.textContent = category.name;
                div.appendChild(nameCategory);

                const btnChangeCategory = createButton('Изменить', () => renderCategoryForm('Change', category));
                div.appendChild(btnChangeCategory);

                const btnDeleteCategory = createButton('Удалить', () => renderCategoryForm('Delete', category));
                div.appendChild(btnDeleteCategory);

                modalCategoryBox.appendChild(div);
            })
        }
        const btnAddCategory = createButton(' + ', () => renderCategoryForm('Add'));
        modalCategoryBox.appendChild(btnAddCategory);
    }
    catch (error) {
        console.error('Ошибка загрузки категорий:', error);
        alert('Не удалось загрузить категории');
    }
    
}

//Создание формы для создания/изменения категории
function formCategories(type, category) {
    
    const h3 = document.createElement('h3');
    if (type === 'Add') h3.textContent = 'Новая категория';
    else h3.textContent = `Редактировать категорию: ${category.name}`;
    modalCategoryBox.appendChild(h3);

    const form = document.createElement('form');

    const label = document.createElement('label');
    label.textContent = 'Название категории:';
    label.for = 'nameCategory';

    const input = document.createElement('input');
    input.id = 'nameCategory';
    input.name = 'Name';
    input.type = 'text';
    input.required = true;
    input.minLength = 2;
    input.maxLength = 50;
    input.placeholder = 'Например: Завод'

    if (type === 'Change') input.value = `${category.name}`;

    form.appendChild(label);
    form.appendChild(input);

    modalCategoryBox.appendChild(form);

    const div = document.createElement('div');
    div.className = 'btnsAction';

    const btnNOSaveNewCategory = createButton('Отмена', () => renderCategoryForm('LoadList'));
    div.appendChild(btnNOSaveNewCategory);

    let btnSaveCategory;
    if (type === 'Add') btnSaveCategory = createButton('Сохранить', async () => submitCategory('Add'));
    else btnSaveCategory = createButton('Сохранить', async () => submitCategory('Change', category.id));
    div.appendChild(btnSaveCategory);

    modalCategoryBox.appendChild(div);
}

//Подтверждение удаления
function confirmationDeletion(category) {
    const p = document.createElement('p');
    p.innerHTML = `Вы уверены, что хотите удалить категорию: <strong>${escapeHtml(category.name) }</strong>?
    <span>После удаления данной категории, места с этой категорией получат пустую категорию.</span>`
    modalCategoryBox.appendChild(p);

    const div = document.createElement('div');
    div.className = 'btnsAction';

    const btnNODeleteCategory = createButton('Отмена', () => renderCategoryForm('LoadList'));
    div.appendChild(btnNODeleteCategory);

    const btnDeleteCategory = createButton('Удалить', async () => deleteCategories(category.id));
    div.appendChild(btnDeleteCategory);

    modalCategoryBox.appendChild(div);
}

//Отправка запроса на создание/изменение
async function submitCategory(type, id = null) {
    const input = document.getElementById('nameCategory');
    const name = input.value.trim();

    if (!name || name.length < 2) {
        alert('Название категории должно быть не менее 2 символов.');
        return;
    }

    buttonLock();

    try {
        const url = type === 'Add'
            ? '/api/Api/AddCategory'
            : `/api/Api/ChangeCategory/${id}`;

        const method = type === 'Add' ? 'POST' : 'PUT';

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name })
        });

        await handleApiResponse(response);

    } catch (error) {
        console.error('Ошибка:', error);
        alert(`Не удалось ${type === 'Add' ? 'сохранить' : 'изменить'} категорию: ${error.message}`);
    } finally {
        buttonUnlock();
    }
}

//Отправка запроса на удаление
async function deleteCategories(id = null) {


    if (id === null) {
        alert('Ошибка удаления');
        return;
    }
    buttonLock();
    try {
        const response = await fetch(`/api/Api/DeleteCategory/${id}`, {
            method: 'DELETE'
        });
        await handleApiResponse(response);
    }
    catch (error) {
        console.error('Ошибка:', error);
        alert('Не удалось удалить категорию');
    }
    finally {
        buttonUnlock();
    }
}

//Блокировка кнопок
function buttonLock() {
    const btnsModal = modalCategoryBox.querySelectorAll('.btnModal');
    btnsModal.forEach(
        btn => {
            btn.disabled = true;
        }
    )
}

//Разблокировка кнопок
function buttonUnlock() {
    const btnsModal = modalCategoryBox.querySelectorAll('.btnModal');
    btnsModal.forEach(
        btn => {
            btn.disabled = false;
        }
    )
}

//Обработка ответа
async function handleApiResponse(response) {
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Ошибка операции');
    }
    await renderCategoryForm('LoadList');
}

//Функция создания кнопок
function createButton(text, onClick) {
    const btn = document.createElement('button');
    btn.textContent = text;
    btn.className = 'btnModal';
    btn.addEventListener('click', onClick);
    return btn;
}

//Функция экранирования
function escapeHtml(unsafe) {
    return unsafe.replace(/[&<>"']/g, function (m) {
        return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#039;' }[m];
    });
}