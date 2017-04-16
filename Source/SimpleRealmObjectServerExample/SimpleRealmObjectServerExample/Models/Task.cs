using Realms;

namespace SimpleRealmObjectServerExample.Models
{
    public class Task : RealmObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
