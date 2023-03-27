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

        List<Tag> tags = new List<Tag>(); //מה שהתחלנו

        public int PostId { get => postId; set => postId = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string Type { get => type; set => type = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public List<Tag> Tags { get => tags; set => tags = value; } //מה שהתחלנו


        public static List<SocialCloud> ReadByGroupId(int groupId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.ReadByGroupId(groupId);
        }

        public int Insert(string tagsJson)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.InsertSocialCloud(this, tagsJson);
        } 

        public static int Delete(int postId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.DeleteFromSocialCloud(postId);
        }

     


    }
}
