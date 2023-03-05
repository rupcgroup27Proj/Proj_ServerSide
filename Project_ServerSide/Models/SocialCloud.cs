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

        public int PostId { get => postId; set => postId = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public int TeacherId { get => teacherId; set => teacherId = value; }
        public int GuideId { get => guideId; set => guideId = value; }
        public string FileUrl { get => fileUrl; set => fileUrl = value; }
        public string Type { get => type; set => type = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }

        public static List<SocialCloud> ReadBygroupId(int groupId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.PostsLikesByGroupId(groupId);
        }

        public int Insert()
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.InsertSocialCloud(this);
        }

        public int InsertTagsToPost(int tagId, int postId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.InsertTagsToPost(tagId, postId);

        }
        public static bool DeleteByStudent(int studentId, int postId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.DeleteFromSocialCloudByStudent(studentId, postId);
        }

        public static int DeleteByTeacher(int teacherId, int postId)
        {
            SocialCloud_DBservice dbs = new SocialCloud_DBservice();
            return dbs.DeleteFromSocialCloudByTeacher(teacherId, postId);
        }
    }
}
