# Список интеграционных тестов API

## PropertiesController
- `Create_ValidProperty_ReturnsCreated` - Проверяет, что создание валидного объекта размещения возвращает 201 Created с корректными данными.
- `GetById_ExistingProperty_ReturnsOk` - Проверяет, что запрос существующего объекта по ID возвращает 200 OK и ожидаемое имя.
- `Delete_ExistingProperty_ReturnsNoContent` - Проверяет, что удаление существующего объекта возвращает 204 NoContent, а повторный запрос - 404 NotFound.
- `Create_PropertyWithInvalidCoords_ReturnsBadRequest` - Проверяет, что создание объекта с недопустимыми координатами возвращает 400 BadRequest с ошибками для Latitude и Longitude.
- `Delete_PropertyWithRoomTypes_ReturnsBadRequest` - Проверяет, что попытка удалить объект, имеющий типы номеров, возвращает 409 Conflict с сообщением о невозможности удаления.

## ReservationsController
- `CreateReservation_ValidRequest_ReturnsCreatedWithTotal` - Проверяет, что создание валидной брони возвращает 201 Created с корректно рассчитанной стоимостью и данными гостя.
- `CreateReservation_WhenRoomFullyBooked_ReturnsConflict` - Проверяет, что создание брони при отсутствии доступных номеров возвращает 409 Conflict с сообщением о недоступности.
- `CreateReservation_InvalidDates_ReturnsBadRequest` - Проверяет, что создание брони с датой прибытия позже даты отъезда возвращает 400 BadRequest с соответствующим сообщением.
- `CreateReservation_GuestCountOutOfRange_ReturnsBadRequest` - Проверяет, что создание брони с количеством гостей вне допустимых пределов возвращает 400 BadRequest.
- `CreateReservation_PropertyNotFound_ReturnsNotFound` - Проверяет, что создание брони для несуществующего объекта размещения возвращает 404 NotFound.
- `CreateReservation_RoomTypeNotFound_ReturnsNotFound` - Проверяет, что создание брони для несуществующего типа номера возвращает 404 NotFound.
- `CreateReservation_InvalidDto_ReturnsBadRequest` - Проверяет, что создание брони с невалидными данными DTO (пустое имя, некорректный телефон, нулевые гости) возвращает 400 BadRequest с ошибками валидации.
- `GetReservations_NoFilters_ReturnsAll` - Проверяет, что запрос всех бронирований без фильтров возвращает полный список созданных записей.
- `GetReservations_FilterByPropertyId_ReturnsFiltered` - Проверяет, что фильтрация по PropertyId возвращает только брони для указанного объекта.
- `GetReservationById_Existing_ReturnsOk` - Проверяет, что запрос существующей брони по ID возвращает 200 OK и корректные данные.
- `GetReservationById_NonExisting_ReturnsNotFound` - Проверяет, что запрос несуществующей брони возвращает 404 NotFound.
- `CancelReservation_Existing_ReturnsNoContent` - Проверяет, что отмена существующей брони возвращает 204 NoContent, и бронь более недоступна.
- `CancelReservation_NonExisting_ReturnsNotFound` - Проверяет, что попытка отменить несуществующую бронь возвращает 404 NotFound.

## RoomTypesController
- `CreateRoomType_ValidRequest_ReturnsCreated` - Проверяет, что создание валидного типа номера возвращает 201 Created с правильными данными и привязкой к объекту.
- `CreateRoomType_PropertyNotFound_ReturnsNotFound` - Проверяет, что создание типа номера для несуществующего объекта возвращает 404 NotFound.
- `CreateRoomType_InvalidDto_ReturnsBadRequest` - Проверяет, что создание типа номера с невалидными данными (пустое имя, отрицательная цена и т.д.) возвращает 400 BadRequest с множеством ошибок валидации.
- `GetRoomTypesByProperty_ReturnsList` - Проверяет, что запрос списка типов номеров по PropertyId возвращает правильный набор созданных записей.
- `GetRoomTypeById_Existing_ReturnsOk` - Проверяет, что запрос типа номера по существующему ID возвращает 200 OK и корректные данные.
- `GetRoomTypeById_NonExisting_ReturnsNotFound` - Проверяет, что запрос типа номера по несуществующему ID возвращает 404 NotFound.
- `UpdateRoomType_ValidRequest_ReturnsNoContent` - Проверяет, что обновление существующего типа номера валидными данными возвращает 204 NoContent и изменяет запись.
- `UpdateRoomType_NonExisting_ReturnsNotFound` - Проверяет, что попытка обновить несуществующий тип номера возвращает 404 NotFound.
- `UpdateRoomType_InvalidDto_ReturnsBadRequest` - Проверяет, что обновление типа номера с невалидными данными возвращает 400 BadRequest с ошибками валидации.
- `DeleteRoomType_WithoutReservations_ReturnsNoContent` - Проверяет, что удаление типа номера без бронирований возвращает 204 NoContent и запись пропадает.
- `DeleteRoomType_WithExistingReservations_ReturnsBadRequest` - Проверяет, что удаление типа номера с активными бронированиями возвращает 400 BadRequest с сообщением о невозможности удаления.
- `DeleteRoomType_NonExisting_ReturnsNotFound` - Проверяет, что попытка удалить несуществующий тип номера возвращает 404 NotFound.

## SearchController
- `Search_ValidCriteria_ReturnsMatchingRoomTypes` - Проверяет, что поиск по городу, датам и количеству гостей возвращает подходящие номера с правильным расчётом стоимости и доступностью.
- `Search_GuestsExceedRoomCapacity_ExcludesRoomType` - Проверяет, что номера с вместимостью меньше указанного количества гостей исключаются из результатов.
- `Search_MaxPriceFilter_ExcludesExpensiveRooms` - Проверяет, что фильтр максимальной цены исключает номера с ценой выше указанной.
- `Search_RoomFullyBooked_ExcludesRoomType` - Проверяет, что полностью занятый номер не появляется в результатах поиска.
- `Search_PartialAvailability_ShowsRemainingRooms` - Проверяет, что при частичной занятости отображается правильное количество оставшихся номеров.
- `Search_CityNotFound_ReturnsEmptyList` - Проверяет, что поиск по несуществующему городу возвращает пустой список.
- `Search_NoMatchingRoomType_ReturnsEmptyList` - Проверяет, что при отсутствии номеров, удовлетворяющих по вместимости, возвращается пустой список.
- `Search_MissingCity_ReturnsBadRequest` - Проверяет, что запрос без обязательного параметра city возвращает 400 BadRequest.
- `Search_InvalidDateRange_ReturnsBadRequest` - Проверяет, что запрос с датой прибытия позже даты отъезда возвращает 400 BadRequest с сообщением об ошибке.
- `Search_MissingArrivalDate_ReturnsBadRequest` - Проверяет, что отсутствие arrivalDate в запросе возвращает 400 BadRequest.
- `Search_MissingDepartureDate_ReturnsBadRequest` - Проверяет, что отсутствие departureDate в запросе возвращает 400 BadRequest.
- `Search_GuestsLessThanOne_ReturnsBadRequest` - Проверяет, что значение guests=0 возвращает 400 BadRequest с ошибкой валидации.
- `Search_NegativeMaxPrice_ReturnsBadRequest` - Проверяет, что отрицательное значение maxPrice возвращает 400 BadRequest с ошибкой валидации.