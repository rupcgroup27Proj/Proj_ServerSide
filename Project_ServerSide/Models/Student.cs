using Project_ServerSide.Models.DAL;

namespace Project_ServerSide.Models
{
    //תיקונים:
    //לשנות את הדאטה בייס להוסיף את כל הפרטים
    //     בקלאס לשנות את פונקציית פבליק
    //    לוודא מול הקונטרולר עד דקה 14
    //    ---כל זה הכנסת משתמש--
    //    אחר כך
    //    לבדוק אם הסיסמא והמייל קיים במערכת
    //    אם כן פצצה ולשרשר את שאר הפרטים
    //    להוסיף TYPE למחלקה!
    //להבין איזה טייפ זה תמונה בקלאס
    public class Student
    {
        double password;
        int studentId;
        string firstName;
        string lastName;
        double phone;
        string email;
        double parentPhone;
        //[pictureUrl]

        public double Password { get => password; set => password = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public double Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public double ParentPhone { get => parentPhone; set => parentPhone = value; }

        public Student(double password,int studentId)// להוסיף את כל הפרטים להזנה ולשים לב לGET
            {
            password = password;
            studentId = studentId;
        }

            public int Insert()//insetrt new students to DB
        {
            Students_DBservices dbs = new Students_DBservices();
                return dbs.Insert(this);
            }

            public int Update()
            {
            Students_DBservices dbs = new Students_DBservices();
                return dbs.Update(this);

            }

            public List<Student> Read()
            {
                Students_DBservices dbs = new Students_DBservices();
                //return dbs.Read();
                return null;
            }

            public void Init()
            {
            //SmartRec_DBservices dbs = new SmartRec_DBservices();
            //dbs.Init();
        }

    }
    }

