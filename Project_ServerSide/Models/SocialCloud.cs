using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class SocialCloud
    {
        int postId;
        int groupId;
        int studentId;
        int teacherId;
        int guideId;
        string fileUrl;
        string type;
        DateTime createdAt;
        string firstName;
        string lastName;
        int likes;
        int comments;
        string description;
        List<Tag> tags = new List<Tag>();


        public int PostId { get => postId; set => postId = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string Type { get => type; set => type = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public int Likes { get => likes; set => likes = value; }
        public int Comments { get => comments; set => comments = value; }
        public string Description { get => description; set => description = value; }
        public List<Tag> Tags { get => tags; set => tags = value; }
       

        public static string ReadSocialCloudByGroupId(int groupId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.ReadSocialCloudByGroupId(groupId);
        }

        public int InsertSocialCloud(string tagsJson)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.InsertSocialCloud(this, tagsJson);
        }

        public static int DeleteFromSocialCloud(int postId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.DeleteFromSocialCloud(postId);
        }
    }
}
