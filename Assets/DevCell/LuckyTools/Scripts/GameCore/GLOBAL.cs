using System.Collections.Generic;
using UnityEngine;

//ДВИЖЕНИЕ
/* + Правильное движение игроков
 * + Правильное движение монстров
 * + Правильные прыжки игроков
 * + Правильные прыжки монстров
 * + Правильная анимация игроков
 * + Правильная анимация монстров
 * Смерть от падения и от выпадения
 * Странное передергивание анимации при движении справа налево
 */

//БОЙ
/* + Слои и теги
 * + Удар рукой
 * + Удар когтями (поправить анимацию)
 * Удар мечем
 * Удар топором
 * Удар киркой
 * + Урон игрокам
 * + Урон монстрам
 * + Мерцание при уроне
 * + Активация монстров только при приближении
 * + Монстры должны преследовать игрока, если рядом
 * + Здоровье
 * + Индикатор здоровья игроков
 * + Индикатор здоровья монстров
 * + Popup урона (поправить вытянутость шрифта)
 * + Восполнение здоровья
 * + Смерть
 * Анимация смерти
 */

//МИР И БЛОКИ
/* + Static блоки
 * + Генерация блоков из картинки
 * + Вспомогательный пиксел для маски глубины
 * + Выбор активного блока
 * + Установка блоков по глубине
 * + Удаление блоков целым рядом (клавиша 0)
 * + Сохранение изменений в мире в картинку (клавиша =)
 * Фоновые деревья в зависимости от биома
 * + Фоновый туман
 * + Фоновые горы
 * День/Ночь
 * + Освещение и тени
 * +- Трава по фиксированному рандому
 * Улучшить креатив режим
 */

//РАЗНОЕ
/* Иногда спавнящиеся Курицы несущие яйца и перья при ударе
 * Поднятие вещей и текущий объект в руках
 * Использование объекта в руках (там где можно)
 * Удар рукой по дереву выбивает только палки
 */

public class GLOBAL : MonoBehaviour
{
    public static float GRAVITY = 6.67f;
    public static GameObject SELECTED_BLOCK = null;
    public static float SLICE_DURATION = 0.167f;
    public static float CLAWS_DURATION = 0.250f;
    public static float PUNCH_DURATION = 0.317f;
    public static float SWORD_DURATION = 0.383f;
    public static float SPEAR_DURATION = 0.317f;
    public static float HATCHET_DURATION = 0.317f;
    public static float PICKER_DURATION = 0.383f;
    public static float SHOVEL_DURATION = 0.167f;
    public static float HIT_DURATION = 0.2f;

    public static Dictionary<string, float> ITEM_CLASSES = new Dictionary<string, float>()
        {
            { "Sword", SWORD_DURATION },
            { "Hatchet", HATCHET_DURATION },
            { "Spear", SPEAR_DURATION },
            { "Shovel", SHOVEL_DURATION },
            { "Picker", PICKER_DURATION },
        };
}
