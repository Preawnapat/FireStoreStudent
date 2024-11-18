using Google.Cloud.Firestore;
using System;

namespace FireStoreStudent.Models;
public class StudentsModel
{
    [FirestoreProperty]
    public string Id { get; set; }


    [FirestoreProperty]
    public string Code { get; set; }


    [FirestoreProperty]
    public string Name { get; set; }






}