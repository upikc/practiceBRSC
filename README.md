### /api/Auth/register

#### POST: Регистрирует нового пользователя

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Успешная регистрация. Возвращает токен доступа |
| 409 | Ошибка. Почта занята |

### /api/Auth/login

#### POST: Авторизация пользователя

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Успешный вход. Возвращает токен доступа |
| 400 | Ошибка. Неверные данные |

### /api/Users/GetUsersWithDetails

#### GET: Получение данных пользователей

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Возвращает данные пользователей |
| 401 | Недостаток токена |
| 403 | Доступ запрещен |

### /api/Users/GetUserById

#### GET: Получение данных пользователя по ID

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

#### PUT: Обновление имени и почты пользователя

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

#### PUT: Обновление имени, почты, пароля и роли пользователя

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

#### DELETE: Удаление пользователя

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
