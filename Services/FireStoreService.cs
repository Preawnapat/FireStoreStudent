using Google.Cloud.Firestore;
using System;
using FireStoreStudent.Models;


namespace FireStoreStudent.Services
{
    public class FirestoreService
    {
        private FirestoreDb _db;
        public string StatusMessage;

        public FirestoreService()
        {
            this.SetupFireStore();
        }

        private async Task SetupFireStore()
        {
            if (_db == null)
            {
                
                var stream = await FileSystem.OpenAppPackageFileAsync("assignment-87181-firebase-adminsdk-c37ww-b7fee7ecc4.json");
                var reader = new StreamReader(stream);
                var contents = reader.ReadToEnd();
                _db = new FirestoreDbBuilder
                {
                    ProjectId = "assignment-87181",

                    JsonCredentials = contents
                }.Build();
            }
        }

     
        public async Task<List<StudentsModel>> GetAllStudents()
        {
            try
            {
                await SetupFireStore();
                var data = await _db.Collection("Students").GetSnapshotAsync();
                var students = data.Documents.Select(doc =>
                {
                    var student = new StudentsModel();
                    student.Id = doc.Id;
                    student.Code = doc.GetValue<string>("Code");
                    student.Name = doc.GetValue<string>("Name");
                    return student;
                }).ToList();
                return students;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
                return null;
        }

        public async Task InsertStudent(StudentsModel student)
        {
            try
            {
                await SetupFireStore();
                var studentData = new Dictionary<string, object>
                {
                    { "Code", student.Code },
                    { "Name", student.Name }
                };
                await _db.Collection("Students").AddAsync(studentData);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
        public async Task UpdateStudent(StudentsModel student)
        {
            try
            {
                await SetupFireStore();

                // Manually create a dictionary for the updated data
                var studentData = new Dictionary<string, object>
            {
             
                { "Code", student.Code },
                { "Name", student.Name },
                { "Id" , student.Id }
                
                // Add more fields as needed
            };

                // Reference the document by its Id and update it
                var docRef = _db.Collection("Students").Document(student.Id);
                await docRef.SetAsync(studentData, SetOptions.Overwrite);

                StatusMessage = "Student successfully updated!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
        // Delete Student
        public async Task DeleteStudent(string id)
        {
            try
            {
                await SetupFireStore();

                var docRef = _db.Collection("Students").Document(id);
                await docRef.DeleteAsync();

                StatusMessage = "Student successfully deleted!";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
            }
        }
    }
}

