using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LicenseApp.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;
using LicenseApp.Services;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using LicenseApp.Views;
using LicenseApp.ViewModels;


namespace LicenseApp.Services
{
    class LicenseAPIProxy
    {
        private const string CLOUD_URL = "TBD"; //API url when going on the cloud
        private const string CLOUD_PHOTOS_URL = "TBD";
        private const string DEV_ANDROID_EMULATOR_URL = "http://10.0.2.2:13134/LicenseAPI"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_URL = "http://10.58.55.51:13134/LicenseAPI"; //API url when using physucal device on android
        private const string DEV_WINDOWS_URL = "http://localhost:13134/LicenseAPI"; //API url when using windoes on development
        private const string DEV_ANDROID_EMULATOR_PHOTOS_URL = "http://10.0.2.2:13134/Images/"; //API url when using emulator on android
        private const string DEV_ANDROID_PHYSICAL_PHOTOS_URL = "http://10.58.55.51:13134/Images/"; //API url when using physucal device on android
        private const string DEV_WINDOWS_PHOTOS_URL = "https://localhost:44318/Images/"; //API url when using windoes on development

        private HttpClient client;
        private string baseUri;
        private string basePhotosUri;
        private static LicenseAPIProxy proxy = null;

        public static LicenseAPIProxy CreateProxy()
        {
            string baseUri;
            string basePhotosUri;
            if (App.IsDevEnv)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (DeviceInfo.DeviceType == DeviceType.Virtual)
                    {
                        baseUri = DEV_ANDROID_EMULATOR_URL;
                        basePhotosUri = DEV_ANDROID_EMULATOR_PHOTOS_URL;
                    }
                    else
                    {
                        baseUri = DEV_ANDROID_PHYSICAL_URL;
                        basePhotosUri = DEV_ANDROID_PHYSICAL_PHOTOS_URL;
                    }
                }
                else
                {
                    baseUri = DEV_WINDOWS_URL;
                    basePhotosUri = DEV_WINDOWS_PHOTOS_URL;
                }
            }
            else
            {
                baseUri = CLOUD_URL;
                basePhotosUri = CLOUD_PHOTOS_URL;
            }

            if (proxy == null)
                proxy = new LicenseAPIProxy(baseUri, basePhotosUri);
            return proxy;
        }

        private LicenseAPIProxy(string baseUri, string basePhotosUri)
        {
            //Set client handler to support cookies!!
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new System.Net.CookieContainer();

            //Create client with the handler!
            this.client = new HttpClient(handler, true);
            this.baseUri = baseUri;
            this.basePhotosUri = basePhotosUri;
        }

        public string GetBasePhotoUri() { return this.basePhotosUri; }

        //פעולה המקבלת אימייל וסיסמה, מחברת את המשתמש לאפליקציה ומחזירה אובייקט לפי סוג המשתמש
        public async Task<Object> LoginAsync(string email, string pass)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/Login?email={email}&pass={pass}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    try //בדיקה אם המשתמש התחבר בתור מנהל בית ספר
                    {
                        SchoolManager sm = JsonSerializer.Deserialize<SchoolManager>(content, options);
                        if (sm.Smname != null)
                            return sm;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    try //בדיקה אם המשתמש התחבר בתור מורה נהיגה
                    {
                        Instructor i = JsonSerializer.Deserialize<Instructor>(content, options);
                        if (i.Iname != null)
                            return i;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    try //בדיקה אם המשתמש התחבר בתור תלמיד
                    {
                        Student s = JsonSerializer.Deserialize<Student>(content, options);
                        if (s.Sname != null)
                            return s;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> Logout()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                App app = (App)App.Current;
                string jsonObject = JsonSerializer.Serialize<Object>(app.CurrentUser, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/Logout", content);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        //פעולה המחזירה את כל טבלאות הנתונים הקבועות
        public async Task<LookupTables> GetLookups()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLookups");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    LookupTables table = JsonSerializer.Deserialize<LookupTables>(content, options);
                    return table;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של עיר ומחזירה את האובייקט של העיר המתאימה
        public async Task<City> GetCityById(int cityId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetCityById?cityId={cityId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    City city = JsonSerializer.Deserialize<City>(content, options);
                    return city;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של אזור ומחזירה את האובייקט של האזור המתאים
        public async Task<Area> GetAreaById(int areaId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetAreaById?areaId={areaId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Area area = JsonSerializer.Deserialize<Area>(content, options);
                    return area;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של שעה ומחזירה את האובייקט של השעה המתאימה
        public async Task<WorkingHour> GetWorkingHourById(int hourId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetWorkingHourById?hourId={hourId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    WorkingHour hour = JsonSerializer.Deserialize<WorkingHour>(content, options);
                    return hour;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של תיבת הילוכים ומחזירה את האובייקט של תיבת ההילוכים המתאימה
        public async Task<Gearbox> GetGearboxById(int gearboxId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetGearboxById?gearboxId={gearboxId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Gearbox gearbox = JsonSerializer.Deserialize<Gearbox>(content, options);
                    return gearbox;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של אורך שיעור ומחזירה את האובייקט של אורך השיעור המתאים
        public async Task<LessonLength> GetLessonLengthById(int lessonLenghId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLessonLengthById?lessonLenghId={lessonLenghId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    LessonLength lessonLength = JsonSerializer.Deserialize<LessonLength>(content, options);
                    return lessonLength;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מחרוזת של שעה ומחזירה את האובייקט של השעה המתאימה
        public async Task<WorkingHour> GetHour(string wHour)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetHour?wHour={wHour}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    WorkingHour wh = JsonSerializer.Deserialize<WorkingHour>(content, options);
                    return wh;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מחזירה רשימה של כל המורים הרשומים באפליקציה
        public async Task<ObservableCollection<Instructor>> GetAllInstructorsAsync()
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetInstructors");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Instructor> instructors = JsonSerializer.Deserialize<ObservableCollection<Instructor>>(content, options);
                    return instructors;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של תלמיד ומחזירה רשימה של כל השיעורים שלו
        public async Task<ObservableCollection<Lesson>> GetStudentLessonsAsync(int studentId)
        {
            try
            {
                 HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLessons?studentId={studentId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Lesson> lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(content, options);
                    return lessons;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של מורה ומחזירה רשימה של כל הביקורות שנכתבו עליו
        public async Task<ObservableCollection<Review>> GetInstructorReviewsAsync(int instructorID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetInstructorReviews?instructorID={instructorID}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Review> reviews = JsonSerializer.Deserialize<ObservableCollection<Review>>(content, options);
                    return reviews;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מספר מזהה של מורה ומחזירה את כל התלמידים המשויכים אליו
        public async Task<ObservableCollection<Student>> GetStudentsByInstructorAsync(int instructorID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetStudentsByInstructor?instructorId={instructorID}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Student> students = JsonSerializer.Deserialize<ObservableCollection<Student>>(content, options);
                    return students;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המחזירה רשימה של כל התלמידים שהגישו בקשה להירשם אצל מורה מסוים ועדיין מחכים לתשובה
        public async Task<ObservableCollection<Student>> GetWaitingStudentsByInstructor(int instructorID)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetWaitingStudentsByInstructor?instructorId={instructorID}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Student> students = JsonSerializer.Deserialize<ObservableCollection<Student>>(content, options);
                    return students;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת תלמיד חדש ורושמת אותו לאפליקציה 
        public async Task<Student> StudentSignUpAsync(Student student)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Student>(student, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUpStudent", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Student s = JsonSerializer.Deserialize<Student>(jsonObject, options);
                    return s;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            
        }

        //פעולה המעדכנת את פרטי התלמיד המחובר
        public async Task<Student> UpdateStudent(Student student)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Student>(student, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateStudent", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Student ret = JsonSerializer.Deserialize<Student>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מעדכנת את פרטיו של המנהל בית ספר המחובר
        public async Task<SchoolManager> UpdateSManager(SchoolManager schoolManager)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<SchoolManager>(schoolManager, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateSManager", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    SchoolManager ret = JsonSerializer.Deserialize<SchoolManager>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו מקבלת מורה חדש ורושמת אותו לאפליקציה
        public async Task<Instructor> InstructorSignUpAsync(Instructor instructor)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Instructor>(instructor, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUpInstructor", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Instructor i = JsonSerializer.Deserialize<Instructor>(jsonObject, options);
                    return i;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        //פעולה זו מקבלת מנהל בית ספר חדש ורושמת אותו לאפליקציה
        public async Task<SchoolManager> SchoolManagerSignUpAsync(SchoolManager sManager)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<SchoolManager>(sManager, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/SignUpSchoolManager", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    SchoolManager sm = JsonSerializer.Deserialize<SchoolManager>(jsonObject, options);
                    return sm;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        //פעולה זו מקבלת בית ספר חדש לנהיגה ומוסיפה אותו למערכת
        public async Task<DrivingSchool> AddDrivingSchool(DrivingSchool drivingSchool)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<DrivingSchool>(drivingSchool, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddDrivingSchool", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    DrivingSchool ds = JsonSerializer.Deserialize<DrivingSchool>(jsonObject, options);
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        //פעולה המקבלת בקשת רישום חדשה ושומרת אותה במערכת
        public async Task<EnrollmentRequest> AddEnrollmentRequest(EnrollmentRequest enrollmentRequest)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<EnrollmentRequest>(enrollmentRequest, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddEnrollmentRequest", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    EnrollmentRequest er = JsonSerializer.Deserialize<EnrollmentRequest>(jsonObject, options);
                    return er;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        //פעולה המקבלת ביקורת חדשה ושומרת אותה במערכת
        public async Task<Review> AddReview(Review review)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Review>(review, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddReview", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Review r = JsonSerializer.Deserialize<Review>(jsonObject, options);
                    return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המקבלת סיכום שיעור חדש ושומרת אותו במערכת
        public async Task<StudentSummary> AddSummary(StudentSummary summary)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<StudentSummary>(summary, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddSummary", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    StudentSummary r = JsonSerializer.Deserialize<StudentSummary>(jsonObject, options);
                    return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המקבלת ביקורת חדשה על מורה מסויים ושומרת אותה במערכת
        public async Task<InstructorReview> AddInstructorReview(InstructorReview review)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<InstructorReview>(review, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddInstructorReview", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    InstructorReview r = JsonSerializer.Deserialize<InstructorReview>(jsonObject, options);
                    return r;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המעדכנת את פרטיו הנוכחיים של המורה המחובר
        public async Task<Instructor> UpdateInstructor(Instructor instructor)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Instructor>(instructor, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateInstructor", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Instructor ret = JsonSerializer.Deserialize<Instructor>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המקבלת אובייקט המאפיין תמונה ושומרת את התמונה בתיקיה
        public async Task<bool> UploadImage(Models.FileInfo fileInfo, string targetFileName)
        {
            try
            {
                var multipartFormDataContent = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.Name));
                multipartFormDataContent.Add(fileContent, "file", targetFileName);
                HttpResponseMessage response = await client.PostAsync($"{this.baseUri}/UploadImage", multipartFormDataContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        //פעולה המנתקת קשר בין תלמיד למורה שלו
        public async Task<Student> DeleteStudent(int studentId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/DeleteStudent?studentId={studentId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Student student = JsonSerializer.Deserialize<Student>(content, options);
                    return student;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המנתקת קשר בין מורה לבית הספר שמשויך אליו
        public async Task<Instructor> DeleteInstructor(int instructorId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/DeleteInstructor?instructorId={instructorId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Instructor instructor = JsonSerializer.Deserialize<Instructor>(content, options);
                    return instructor;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המחזירה רשימה של כל התלמידים המשויכים לבית ספר נהיגה מסוים
        public async Task<ObservableCollection<Student>> GetStudentsBySchoolAsync(int sManagerId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetStudentsBySchool?sManagerId={sManagerId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Student> students = JsonSerializer.Deserialize<ObservableCollection<Student>>(content, options);
                    return students;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המשנה את הסטטוס ההצטרפות של מורה מסוים או סטטוס הרישום של תלמיד מסוים, תלוי באובייקט שקיבלה
        public async Task<bool> ChangeUserStatus(object u)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                if (u is Instructor)
                {
                    Instructor t = (Instructor)u;
                    string jsonObject = JsonSerializer.Serialize<Instructor>(t, options);
                    StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/ChangeInstructorStatus", content);


                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                else if (u is Student)
                {
                    Student s = (Student)u;
                    string jsonObject = JsonSerializer.Serialize<Student>(s, options);
                    StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/ChangeStudentStatus", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        //פעולה המוסיפה סיכום שיעור חדש לשיעור מסוים
        public async Task<Lesson> UpdateLessonSum(Lesson lesson)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Lesson>(lesson, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/UpdateLessonSum", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Lesson ret = JsonSerializer.Deserialize<Lesson>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המקבלת מספר מזהה של מורה ומחזירה את המורה עצמו
        public async Task<Instructor> GetInstructorById(int instructorId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetInstructorById?instructorId={instructorId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    Instructor instructor = JsonSerializer.Deserialize<Instructor>(content, options);
                    return instructor;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המבטלת שיעור שעדיין לא התקיים או דוחה את הבקשה לקיומו
        public async Task<Lesson> CancelLesson(Lesson lesson)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Lesson>(lesson, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/CancelLesson", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Lesson ret = JsonSerializer.Deserialize<Lesson>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המחזירה רשימה של כל השיעורים של מורה מסוים
        public async Task<ObservableCollection<Lesson>> GetInstructorLessonsAsync(int instructorId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetInstructorLessons?instructorId={instructorId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    ObservableCollection<Lesson> lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(content, options);
                    return lessons;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה המחזירה רשימה של כל התלמידים החדשים שהצטרפו בתקופה מסוימת לאפליקציה, לפי המספר הנקלט
        public async Task<List<Student>> GetNewStudents(int num)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetNewStudents?num={num}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    string content = await response.Content.ReadAsStringAsync();
                    List<Student> students = JsonSerializer.Deserialize<List<Student>>(content, options);
                    return students;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה הבודקת אם שיעור פנוי
        public async Task<bool> CheckIfAvailable(Lesson l)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                string jsonObject = JsonSerializer.Serialize<Lesson>(l, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/CheckIfAvailable", content);


                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        //פעולה המקבלת שיעור חדש שנקבע ושומרת אותו במערכת
        public async Task<Lesson> AddNewLesson(Lesson lesson)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Lesson>(lesson, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/AddNewLesson", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Lesson l = JsonSerializer.Deserialize<Lesson>(jsonObject, options);
                    return l;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

        }

        //פעולה זו משנה את סטטוס השיעור הנקלט למאושר
        public async Task<Lesson> ApproveLesson(Lesson lesson)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };
                string jsonObject = JsonSerializer.Serialize<Lesson>(lesson, options);
                StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/ApproveLesson", content);
                if (response.IsSuccessStatusCode)
                {
                    jsonObject = await response.Content.ReadAsStringAsync();
                    Lesson ret = JsonSerializer.Deserialize<Lesson>(jsonObject, options);
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו בודקת אם כבר קיים משתמש באפליקציה בעל מייל זהה
        public async Task<bool> CheckIfMailExists(string mail)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/CheckIfMailExists?mail={mail}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        //פעולה זו מחזירה את תוכן סיכום השיעור
        public async Task<string> GetLessonSumAsync(int reviewId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/GetLessonSum?reviewId={reviewId}");
                if (response.IsSuccessStatusCode)
                {
                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve, //avoid reference loops!
                        PropertyNameCaseInsensitive = true
                    };
                    //string content = await response.Content.ReadAsStringAsync();
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        //פעולה זו בודקת אם כבק נכתב סיכום שיעור לשיעור מסוים
        public async Task<bool> CheckIfSumExists(int lessonId)
        {
            try
            {
                HttpResponseMessage response = await this.client.GetAsync($"{this.baseUri}/CheckIfSumExists?lessonId={lessonId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }

        //פעולה זו מעדכנת את הדירוג של מורה מסוים
        public async Task<bool> ChangeRating(Instructor instructor)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.Hebrew, UnicodeRanges.BasicLatin),
                    PropertyNameCaseInsensitive = true
                };

                    string jsonObject = JsonSerializer.Serialize<Instructor>(instructor, options);
                    StringContent content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await this.client.PostAsync($"{this.baseUri}/ChangeRating", content);


                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return false;
            }
        }
    }
}
