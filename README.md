### /api/Auth/register

#### POST
##### Summary:

Регистрирует нового пользователя

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Успешная регистрация. Возвращает токен доступа |
| 409 | Ошибка. Почта занята |

### /api/Auth/login

#### POST
##### Summary:

Авторизация пользователя

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Успешый вход. Возвращает токен доступа |
| 400 | Ошибка. Не верные данные |

### /api/Users/GetUsersWithDetails

#### GET
##### Summary:

Получение данных пользователей

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Возвращает данные пользователей |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |

### /api/Users/GetUserById

#### GET
##### Summary:

Получение данных пользователя по ID

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Возвращает данные пользователя |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |
| 404 | Пользователь не найден |

### /api/Users/UpdateUserData

#### PUT
##### Summary:

Обновление имени и почты пользователя

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Обновление успешно |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |
| 404 | Пользователь не найден |

### /api/Users/UpdateUserDataWithPassword

#### PUT
##### Summary:

Обновление имени, почты, пароля и роли пользователя

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Обновление успешно |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |
| 404 | Пользователь не найден |

### /api/Users/DeleteUser

#### DELETE
##### Summary:

Удаление пользователя

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | query |  | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Удаление успешно |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |
| 404 | Пользователь не найден |
