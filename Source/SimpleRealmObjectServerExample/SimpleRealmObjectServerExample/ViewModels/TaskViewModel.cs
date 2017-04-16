using SimpleRealmObjectServerExample.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace SimpleRealmObjectServerExample.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        Task Task;

        public string Title { get { return Task.Title; } }
        public string Description { get { return Task.Description; } }

        public bool IsComplete
        {
            get
            {
                return Task.IsComplete;
            }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand ToggleIsCompleteCommand { get; set; }

        public TaskViewModel(Task task)
        {
            Task = task;
            DeleteCommand = new Command(OnDelete);
            ToggleIsCompleteCommand = new Command(OnToggleComplete);
        }

        private void OnToggleComplete(object obj)
        {
            MessagingCenter.Send(this, "UPDATEISCOMPLETE", Task);
        }

        private void OnDelete(object obj)
        {
            MessagingCenter.Send(this, "DELETE", Task);
        }

        public void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
