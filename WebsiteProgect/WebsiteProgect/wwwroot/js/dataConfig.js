const DATA_TYPES = {
    Category: {
        title: 'Редактор категорий',
        newTitle: 'Новая категория',
        editTitle: (name) => `Редактировать категорию: ${name}`,
        placeholder: 'Завод',
        api: {
            list: '/api/Api/GetCategories',
            add: '/api/Api/AddCategory',
            change: (id) => `/api/Api/ChangeCategory/${id}`,
            delete: (id) => `/api/Api/DeleteCategory/${id}`
        },
        deleteWarning: (name) => `Вы уверены, что хотите удалить категорию: <strong>${escapeHtml(name)}</strong>?
        <span>После удаления данной категории, места с этой категорией получат категорию "Другое".</span>`,
        body: (name) => JSON.stringify({ name })
    },
    Country: {
        title: 'Редактор стран',
        newTitle: 'Новая страна',
        editTitle: (name) => `Редактировать страну: ${name}`,
        placeholder: 'Россия',
        api: {
            list: '/api/Api/GetCountries',
            add: '/api/Api/AddCountry',
            change: (id) => `/api/Api/ChangeCountry/${id}`,
            delete: (id) => `/api/Api/DeleteCountry/${id}`
        },
        deleteWarning: (name) => `Вы уверены, что хотите удалить страну: <strong>${escapeHtml(name)}</strong>?
        <span>После удаления данной страны, города, привязанные к этой стране будут удалены.</span>`,
        body: (name) => JSON.stringify({ name })
    },
    City: {
        title: 'Редактор городов',
        newTitle: 'Новый город',
        editTitle: (name) => `Редактировать город: ${name}`,
        placeholder: 'Москва',
        api: {
            list: '/api/Api/GetCities',
            add: '/api/Api/AddCity',
            change: (id) => `/api/Api/ChangeCity/${id}`,
            delete: (id) => `/api/Api/DeleteCity/${id}`
        },
        deleteWarning: (name) => `Вы уверены, что хотите удалить этот город: <strong>${escapeHtml(name)}</strong>?
        <span>После удаления данного города, места с этим городом получат пустой город.</span>`,
        body: (name, parentId) => JSON.stringify({ name, countryId: parseInt(parentId) }),
        hasParent: true,
        parentType: 'Country',
        parentApi: '/api/Api/GetCountries',
        parentLabel: 'Страна:'
    }
};