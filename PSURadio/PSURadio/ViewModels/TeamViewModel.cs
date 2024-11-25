﻿using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

public class TeamViewModel
{
    public ObservableCollection<TeamMember> TeamMembers { get; set; }
    public ICommand WorkerTappedCommand { get; }

    public TeamViewModel()
    {
        TeamMembers = new ObservableCollection<TeamMember>
        {
            new TeamMember { Name = "Полина", Position = "Руководитель", Description = "1. Кто ты? Я руководитель Студенческого медиацентра, редактор Радио ПГУ.\r\n\r\n2. С днем рождения поздравляйте: От души и с улыбкой на лице.\r\n\r\n3. Пицца или роллы? Конечно же пицца.\r\n\r\n4. Ситуация: ты встретил Сергея Лазарева, твои действия? Как бы это не звучало странно, но я сфотографируюсь с ним и скажу какой он классный и только он достоен победы на Евровидении.\r\n\r\n5. Как часто ты делаешь фотографии? Частенько, я люблю фотографироваться и фотографировать свою сестру, только вот фотоаппарата хорошего нет, но надеюсь, что скоро появится.\r\n\r\n6. Майкл Джексон или Мерлин Менсон? Майкл Джексон, обожаю его лунную походку.\r\n\r\n7. Как часто ты пьешь чай? Каждый день и не по одному разу, я люблю чай особенно с бергамотом и мятой.\r\n\r\n8. Твой главный недостаток? Я совсем не говорю по английски, потому что я немка, а хочется хотя бы немного научиться читать.\r\n\r\n9. У тебя есть 10000000$, куда потратишь? На свою семью и благое дело", ImageUrl = "worker1.jpg" },
            new TeamMember { Name = "Анастасия", Position = "Радиоведущая", Description = "1. Кто ты? Меня зовут Анастасия. Я стильная, добрая и активная. Рисую, пишу стихи и веду свой блог.  \r\n\r\n2. С днем рождения поздравляйте: \r\n\r\n3. Пицца или роллы? Мне нравятся роллы, люблю пробовать разные доставки и делать обзоры в Инстаграм.\r\n\r\n4. Ситуация: ты встретил Сергея Лазарева, твои действия? \r\n\r\n5. Как часто ты делаешь фотографии? Ежедневно. Мне нравится запоминать моменты и создавать из них историю.\r\n\r\n6. Майкл Джексон или Мерлин Менсон? Оба исполнителя впечатляют, но песни Джексона наполняют энергией, под них хочется танцевать.\r\n\r\n7. Как часто ты пьешь чай? Каждый день несколько пакетиков, во время сессии их число постоянно растёт😅\r\n\r\n8. Твой главный недостаток? \r\n\r\n9. У тебя есть 10000000$, куда потратишь? Буду инвестировать, чтобы удвоить или утроить сумму.", ImageUrl = "worker2.jpg" },
            new TeamMember { Name = "Арина", Position = "SMM-специалист", Description = "1. Кто ты? Санникова Арина Алексеевна, студентка 2 курса филологического факультета, специальность филология.\r\n\r\n2. С днем рождения поздравляйте: Прошу поздравлять меня 12 мая в 20:15.\r\n\r\n3. Пицца или роллы? Роллы.\r\n\r\n4. Ситуация: ты встретил Сергея Лазарева, твои действия? Убегу от него.\r\n\r\n5. Как часто ты делаешь фотографии? Каждый день и много.\r\n\r\n6. Майкл Джексон или Мерлин Менсон? Мерлин Менсон.\r\n\r\n7. Как часто ты пьешь чай? Каждый день, по несколько кружек.\r\n\r\n8. Твой главный недостаток? Эмоциональная.\r\n\r\n9. У тебя есть 10000000$, куда потратишь? Родителям помогу и Родине (в приюты, фонды и тд).", ImageUrl = "worker3.jpg" },
            new TeamMember { Name = "Диана", Position = "Фотограф", Description = "1. Кто ты? Диана Кулагина. \r\n\r\n2. С днем рождения поздравляйте: 19 декабря. \r\n\r\n3. Пицца или роллы? Роллы.\r\n\r\n4. Ситуация: ты встретил Сергея Лазарева, твои действия? Попрошу автограф ( его можно отдать другу который от него фанатеет) ну и наверное спросила бы что-то.\r\n\r\n5. Как часто ты делаешь фотографии? Несколько раз в неделю.\r\n\r\n6. Майкл Джексон или Мерлин Менсон? Майкл Джексон (в детстве он мне нравился).\r\n\r\n7. Как часто ты пьешь чай? Минимум 4 кружки в день.\r\n\r\n8. Твой главный недостаток? Сомнения.\r\n\r\n9. У тебя есть 10000000$, куда потратишь? На путешествие.", ImageUrl = "worker4.jpg" },
            //new TeamMember { Name = "Имя Фамилия 5", Position = "Должность 5", Description = "Краткое описание работника 5", ImageUrl = "worker5.jpg" },
            //new TeamMember { Name = "Имя Фамилия 6", Position = "Должность 6", Description = "Краткое описание работника 6", ImageUrl = "worker6.jpg" },
            //new TeamMember { Name = "Имя Фамилия 7", Position = "Должность 7", Description = "Краткое описание работника 7", ImageUrl = "worker7.jpg" },
            //new TeamMember { Name = "Имя Фамилия 8", Position = "Должность 8", Description = "Краткое описание работника 8", ImageUrl = "worker8.jpg" }
        };
        WorkerTappedCommand = new Command<TeamMember>(OnWorkerTapped);
    }
    private async void OnWorkerTapped(TeamMember teamMember)
    {
        if (teamMember == null)
            return;

        // Открытие страницы деталей новости
        await Application.Current.MainPage.Navigation.PushAsync(new TeamMemberPage(teamMember));
    }
}