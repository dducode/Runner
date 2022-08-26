<h1 align="center">Runner</h1>
<h2 align="center">

![a](Assets/Icons/icon.png)

[Runner Demo (.apk file)](https://drive.google.com/drive/folders/1dwZE-FzEZFK_5DWuO4rqVQcMVAV-Mb34?usp=sharing)
</h2>

# Содержание
* [Описание](#описание)
* [Реализация](#реализация)
    * [Менеджеры](#менеджеры)
    * [Система событий](#система-событий)
* [Особенности](#особенности)
    * [Шифрование данных](#шифрование-данных)
    * [Простая генерация игрового уровня](#простая-генерация-игрового-уровня)
* [Использованные ассеты](#использованные-ассеты)

# Описание
Игра реализована на базе движка Unity (v.2021.3.6f1). Суть - вы бежите бесконечно вперёд, собирая монеты и бонусы на своём пути, обходя препятствия и набирая очки.

![b](Screenshots/Screenshot_20220826-230701_Runner.jpg)

# Реализация
## Менеджеры
[GameManager](Assets/Scripts/Managers/GameManager.cs) - реализует основную логику игры - загружает, сохраняет и хранит игровые настройки, управляет другими менеджерами, игровым состоянием (пауза, выход из игры), загружает сцены.

[AudioManager](Assets/Scripts/Managers/AudioManager.cs) предназначен для воспроизведения звуков и музыки в игре.

[UIManager](Assets/Scripts/Managers/UIManager.cs), исходя из названия, управляет пользовательским интерфейсом

[DataManager](Assets/Scripts/Managers/DataManager.cs) отвечает на загрузку, сохранение и доступ к данным игрока.

## Система событий

Для рассылки сообщений я реализовал собственную [широковещательную рассылку сообщений](Assets/Scripts/Broadcast%20messages/BroadcastMessages.cs) на базе event C#, использующую словарь. В словарь в качестве ключа записывается тип сообщения, реализованный в структуре [Messages](Assets/Scripts/Broadcast%20messages/Messages.cs), в качестве значения - экземпляр класса [Message](Assets/Scripts/Broadcast%20messages/Message.cs), в который затем передаётся подписчик в качестве Action делегата.

# Особенности 
## Шифрование данных
Все игровые данные при сохранении шифруются с помощью 128-битного шифрования. Во время игры данные шифруются с помощью XOR и base64-кодирования, используя случайный сессионный ключ, что защищает их от простого взлома ([ссылка на пост](https://habr.com/ru/post/249681/)).

## Простая генерация игрового уровня
Игровая карта генерируется из 3 заранее готовых чанков, которые выбираются случайным образом.

# Использованные ассеты
* Окружение - [раз](https://quaternius.com/index.html), [два](https://assetstore.unity.com/packages/3d/environments/fantasy/a-piece-of-nature-40538)
* [Игровой персонаж](https://assetstore.unity.com/packages/3d/characters/viass-free-character-pack-141471)
* [Звуковые эффекты и музыка](https://mixkit.co/)
* [UI](https://assetstore.unity.com/packages/2d/gui/icons/simple-button-set-01-153979)