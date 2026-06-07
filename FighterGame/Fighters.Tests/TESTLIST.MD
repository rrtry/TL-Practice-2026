# Список тестов с названиями методов

## FighterCreatorTests

- Создание бойца с корректным вводом имени и всех выборов возвращает правильного бойца  
  `CreateFighter_ReadsNameAndChoices_ReturnsCorrectFighter`
- При вводе некорректного имени (пустая строка, пробелы) запрашивает повторно до получения валидного  
  `CreateFighter_InvalidName_RetriesUntilValid`
- При некорректном выборе расы (не число, выход за границы) повторяет запрос до корректного значения  
  `CreateFighter_InvalidRaceSelection_RetriesUntilValid`
- При некорректном выборе класса повторяет запрос до корректного значения  
  `CreateFighter_InvalidClassSelection_RetriesUntilValid`
- При некорректном выборе оружия повторяет запрос до корректного значения  
  `CreateFighter_InvalidWeaponSelection_RetriesUntilValid`
- При некорректном выборе брони повторяет запрос до корректного значения  
  `CreateFighter_InvalidArmorSelection_RetriesUntilValid`

## FighterTests

- Максимальное здоровье равно сумме здоровья расы и класса  
  `GetMaxHealth_ReturnsSumOfRaceAndClassHealth`
- Урон равен сумме урона расы, класса и оружия  
  `CalculateDamage_ReturnsSumOfRaceClassAndWeaponDamage`
- Броня равна сумме брони расы, класса и надетой брони  
  `CalculateArmor_ReturnsSumOfRaceClassAndArmor`
- Инициатива равна сумме инициативы расы и класса  
  `GetInitiative_ReturnsSumOfRaceAndClassInitiative`
- При получении урона без брони наносится полный урон с учётом случайного множителя, но без крита  
  `TakeDamage_WithoutArmor_AppliesFullDamage`
- При наличии брони урон уменьшается, но не ниже проникающего (10% от атаки)  
  `TakeDamage_WithArmor_ReducesDamageButNotBelowPiercing`
- Когда броня превышает урон атакующего, наносится только проникающий урон  
  `TakeDamage_PiercingDamage_WhenArmorExceedsAttack`
- Критический удар удваивает итоговый урон  
  `TakeDamage_CriticalHit_DoublesDamage`
- Здоровье не опускается ниже нуля при смертельном уроне  
  `TakeDamage_HealthDoesNotGoBelowZero`

## ArenaServiceTests

- Добавление бойца выводит сообщение и добавляет в список  
  `AddFighter_ValidFighter_WritesMessageAndAddsToList`
- Вывод списка бойцов при пустой арене показывает количество 0  
  `ListFighters_EmptyArena_WritesZeroCount`
- Метод доступа к бойцам возвращает бойцов в порядке добавления  
  `GetFighters_ReturnsCurrentFightersInOrder`
- Удаление бойца по корректному индексу удаляет его и выводит сообщения  
  `RemoveFighter_ValidIndex_RemovesFighterAndWritesMessages`
- Вывод списка бойцов при нескольких участниках показывает информацию каждого  
  `ListFighters_WithMultipleFighters_WritesAllFighterInfo`
- Симуляция боя с менее чем двумя бойцами выдаёт предупреждение и не начинается  
  `SimulateBattle_NotEnoughFighters_WritesWarningAndDoesNotStart`
- Бой двух бойцов: один побеждает (здоровье одного падает до 0)  
  `SimulateBattle_TwoFighters_OneWins`
- При создании арены список бойцов пуст  
  `GetFighters_InitiallyEmpty_ReturnsEmptyCollection`
- Боец, убитый до своего хода в раунде, не атакует
  `SimulateBattle_FighterKilledBeforeHisTurn_DoesNotAct`
- Выбор цели использует генератор случайных чисел с корректным диапазоном (меняется при уменьшении числа противников)  
  `SimulateBattle_TargetSelection_UsesRandomCorrectly`
- Точное снижение здоровья с бронёй: проверка серии раундов с фиксированным уроном  
  `SimulateBattle_AccurateHealthReduction_WithArmor`
- При превышении лимита раундов бой останавливается, выводится сообщение, несколько бойцов выживают  
  `SimulateBattle_MaxRoundsExhausted_SeveralSurvivors`
- Удаление бойца с несколькими некорректными попытками ввода в итоге удаляет правильного бойца  
  `RemoveFighter_InvalidThenValidInput_RemovesCorrectly`