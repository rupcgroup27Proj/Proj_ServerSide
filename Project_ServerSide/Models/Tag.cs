using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    public class Tag
    {
        int tagId;
        int groupId;
        string tagName;
        
        public int TagId { get => tagId; set => tagId = value; }
        public int GroupId { get => groupId; set => groupId = value; }
        public string TagName { get => tagName; set => tagName = value; }

        //coreTag?

        public static List<Tag> GetTags()
        {
            Tag_DBservices dbs = new Tag_DBservices();
            return dbs.GetTags();
        }

        public int Insert()
        {
            Tag_DBservices dbs = new Tag_DBservices();
            return dbs.InsertNewTag(this);
        }

        public static List<Tag> ReadTagInPost(int postId)
        {
            Tag_DBservices dbs = new Tag_DBservices();
            return dbs.ReadTagInPost(postId);
        }

        static public List<Tag> GetBuiltInTags()
        {
            Tag_DBservices dbs = new Tag_DBservices();
            return dbs.GetBuiltInTags();
        }



    }
}
