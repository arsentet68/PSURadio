using System;
using System.Windows.Input;
using Xamarin.Forms;
using PSURadio.Models;
using PSURadio.Services;
using PSURadio.Views;

namespace PSURadio.ViewModels
{
    public class TeamMemberViewModel : BaseViewModel
    {
        private readonly NewsService _newsService;
        public TeamMember TeamMember { get; set; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public bool IsMenuVisible { get; set; }

        public TeamMemberViewModel(TeamMember teamMember)
        {
            _newsService = new NewsService();
            TeamMember = teamMember;
            EditCommand = new Command(OnEdit);
            DeleteCommand = new Command(OnDelete);

            // Determine user role and set IsMenuVisible accordingly
            var userRole = SessionService.CurrentAuthResult?.Role ?? 0;
            IsMenuVisible = userRole == 2 || userRole == 3;
        }

        private async void OnEdit()
        {
            TeamMember teamMember = this.TeamMember;
        }

        private async void OnDelete()
        {

        }
        public void UpdateNews(News updatedNews)
        {

        }

        public string Name => TeamMember.Name;
        public string Position => TeamMember.Position;
        public string Description => TeamMember.Description;
        public string ImageUrl => TeamMember.ImageUrl;
    }
}
