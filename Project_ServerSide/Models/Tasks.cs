using Project_ServerSide.Models.DAL;


namespace Project_ServerSide.Models
{
    public class Tasks
    {
        int groupId;
        string name;
        string description;
        DateTime createdAt;
        DateTime due;
        string fileURL;

        public int GroupId { get => groupId; set => groupId = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public string FileURL { get => fileURL; set => fileURL = value; }
        public DateTime Due { get => due; set => due = value; }


        public List<Tasks> ReadTaskList(int groupId)
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return dbs.ReadTaskList(groupId);
        }

        public bool InsertTasksbyTeacher()
        {
            Tasks_DBservices dbs = new Tasks_DBservices();
            return (dbs.InsertTasksbyTeacher(this) == 2) ? true : false;
        }
      
    }
}

