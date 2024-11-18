using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FireStoreStudent.Models;
using FireStoreStudent.Services;
using PropertyChanged;


namespace FireStoreStudent.ViewModels;

[AddINotifyPropertyChangedInterface]
public partial class StudentViewModel
{
    private FirestoreService _firestoreService;

    public ObservableCollection<StudentsModel> Students { get; set; } = new ObservableCollection<StudentsModel>();
    public StudentsModel CurrentStudent { get; set; }

    public ICommand Reset { get; set; }
    public ICommand AddOrUpdateCommand { get; set; }
    public ICommand DeleteCommand { get; set; }

    public StudentViewModel(FirestoreService firestoreService)
    {
        this._firestoreService = firestoreService;
        this.Refresh();
        Reset = new Command(async () =>
        {
            CurrentStudent = new StudentsModel();
            await this.Refresh();
        }
        );
        AddOrUpdateCommand = new Command(async () =>
        {
            await this.Save();
            await this.Refresh();
        });
        DeleteCommand = new Command(async () =>
        {
            await this.Delete();
            await this.Refresh();
        });

    }
    public async Task GetAll()
    {
        Students.Clear();
        var items = await _firestoreService.GetAllStudents();
        foreach (var item in items)
        {
            Students.Add(item);
        }
    }

    public async Task Save()
    {
        if (string.IsNullOrEmpty(CurrentStudent.Id))
        {
            await _firestoreService.InsertStudent(this.CurrentStudent);
        }
        else
        {
            await _firestoreService.UpdateStudent(this.CurrentStudent);
        }
    }

    private async Task Refresh()
    {
        CurrentStudent = new StudentsModel();
        await this.GetAll();
    }

    private async Task Delete()
    {
        await _firestoreService.DeleteStudent(this.CurrentStudent.Id);
    }

}