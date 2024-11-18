using FireStoreStudent.Services;
using FireStoreStudent.ViewModels;

namespace FireStoreStudent;

public partial class StudentPage : ContentPage
{
    public StudentPage()
    {
        InitializeComponent();
        var firestoreService = new FirestoreService();
        BindingContext = new StudentViewModel(firestoreService);
    }
}