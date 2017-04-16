using Realms;
using Realms.Sync;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using SimpleRealmObjectServerExample.Models;
using System.Collections.ObjectModel;

namespace SimpleRealmObjectServerExample.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        Realm Realm;

        public ObservableCollection<TaskViewModel> Tasks
        {
            get;
            set;
        } = new ObservableCollection<TaskViewModel>();

        private string _NewTaskTitle;

        public string NewTaskTitle
        {
            get { return _NewTaskTitle; }
            set
            {
                if (_NewTaskTitle != value)
                {
                    _NewTaskTitle = value;
                    OnPropertyChanged();
                }

            }
        }

        private string _NewTaskDescription;

        public string NewTaskDescription
        {
            get { return _NewTaskDescription; }
            set
            {
                if (_NewTaskDescription != value)
                {
                    _NewTaskDescription = value;
                    OnPropertyChanged();
                }

            }
        }

        private bool _IsLoading;

        public bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateCommand { get; private set; }

        public MainPageViewModel()
        {
            CreateCommand = new Command(OnCreate);
            InitializeMessages();
            InitAsync();
        }

        private void InitializeMessages()
        {
            MessagingCenter.Subscribe<TaskViewModel, Task>(this, "DELETE",
                (sender, task) =>
                {
                    Realm.Write(() =>
                        Realm.Remove(task)
                    );
                });

            MessagingCenter.Subscribe<TaskViewModel, Task>(this, "UPDATEISCOMPLETE",
                (sender, task) =>
                {
                    Realm.Write(() =>
                        task.IsComplete = !task.IsComplete
                     );
                    sender.OnPropertyChanged(nameof(TaskViewModel.IsComplete));
                });
        }

        private void OnCreate(object obj)
        {
            Realm.Write(() =>
                Realm.Add(new Models.Task()
                {
                    Title = NewTaskTitle,
                    Description = NewTaskDescription
                })
            );

            NewTaskTitle = string.Empty;
            NewTaskDescription = string.Empty;
        }

        private async System.Threading.Tasks.Task InitAsync()
        {
            IsLoading = true;
            await InitRealmAsync();
            Load();
            IsLoading = false;
        }

        private void Load()
        {
            var tasks = Realm.All<Models.Task>().AsRealmCollection();
            tasks.SubscribeForNotifications(OnCollectionUpdated);
            OnCollectionUpdated(tasks, null, null);
        }

        private async System.Threading.Tasks.Task InitRealmAsync()
        {
            var user = await GetRealmUserAsync();
            var serverURL = new Uri(Constants.RealmServerUrl);
            var configuration = new SyncConfiguration(user, serverURL);

            Realm = Realm.GetInstance(configuration);
        }

        private void OnCollectionUpdated(IRealmCollection<Task> sender, ChangeSet changes, Exception error)
        {
            //very naive implementation :)
            //OK for demo purposes
            Tasks.Clear();
            foreach (var item in sender)
            {
                Tasks.Add(new TaskViewModel(item));
            }
        }

        private async System.Threading.Tasks.Task<User> GetRealmUserAsync()
        {
            User user = null;
            User.ConfigurePersistence(UserPersistenceMode.NotEncrypted);

            if (User.AllLoggedIn.Count() > 1)
                foreach (var item in User.AllLoggedIn)
                {
                    item.LogOut();
                }

            user = User.Current;
            if (user == null)
            {
                var credentials = Credentials.UsernamePassword(Constants.DefaultUser, 
                    Constants.DefaultPassword, createUser: false);

                var authURL = new Uri(Constants.AuthUrl);
                user = await User.LoginAsync(credentials, authURL);
            }

            return user;
        }

        public void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
