using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.ViewModels
{   
    public class ReportCreationDialogViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private IMaterialServiceProvider _materialServiceProvider;
        private Int32 _number;
        private ObservableCollection<ReportItemWrapper> _requirementList;
        private ObservableCollection<SpecificationVersion> _versionList;
        private Person _author;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, _category;
        private Views.ReportCreationDialog _parentDialog;
        
        public ReportCreationDialogViewModel(DBEntities entities,
                                        IMaterialServiceProvider serviceProvider,
                                        Views.ReportCreationDialog parentDialog) : base()
        {
            _entities = entities;
            _materialServiceProvider = serviceProvider;
            _parentDialog = parentDialog;
            _versionList = new ObservableCollection<SpecificationVersion>();
            _requirementList = new ObservableCollection<ReportItemWrapper>();

            _confirm = new DelegateCommand(
                () => {
                    Report temp = new Report();
                    temp.Author = _author;
                    Batch tempBatch = _materialServiceProvider.GetBatch(_batchNumber);
                    temp.Batch = _entities.Batches.First(btc => btc.ID == tempBatch.ID);
                    temp.Category = "TR";
                    temp.Description = _selectedSpecification.Description;
                    temp.IsComplete = false;
                    temp.Number = _number;
                    temp.SpecificationVersion = _selectedVersion;
                    temp.StartDate = DateTime.Now.ToShortDateString();
                    
                    _entities.GenerateTestList(temp, 
                                            _requirementList.Where(req => req.IsSelected).Select(req => req.Instance));
                    
                    _entities.Reports.Add(temp);
                    _entities.SaveChanges();

                    _parentDialog.ReportInstance = temp;
                    _parentDialog.DialogResult = true;
                },
                () => IsValidInput);
                
            _cancel = new DelegateCommand(
                () => {
                    _parentDialog.DialogResult = false;    
                });
        }
        
        public Person Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }
        
        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        
        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public bool IsValidInput
        {
            get { return true; }
        }
        
        public Int32 Number
        {
            get { return _number; }
            set { _number = value; }
        }
        
        public List<Person> TechList
        {
            get { return new List<Person>(_entities.People.Where(pp => pp.Role == "TL" )); }
        }        

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set 
            { 
                _selectedSpecification = value; 
                
                if (_selectedSpecification != null)
                {
                    _versionList = new ObservableCollection<SpecificationVersion>(
                        _entities.SpecificationVersions.Where(sv => sv.SpecificationID == _selectedSpecification.ID));
                    
                    OnPropertyChanged("VersionList");
                }
                    
                
                else
                    _versionList.Clear();
                    
                SelectedVersion = _versionList.FirstOrDefault(sv => sv.IsMain);
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set 
            { 
                _selectedVersion = value; 
                RequirementList.Clear();
                
                if (_selectedVersion != null)
                {
                    List<Requirement> tempReq = _entities.GenerateRequirementList(_selectedVersion);
                    foreach (Requirement rq in tempReq)
                        RequirementList.Add(new ReportItemWrapper(rq));
                }
                OnPropertyChanged("SelectedVersion");
            }
        }
        
        public List<Specification> SpecificationList
        {
            get { return new List<Specification>(_entities.Specifications); }
        }
        
        public ObservableCollection<SpecificationVersion> VersionList
        {
            get { return _versionList; }
        }
            
        public ObservableCollection<ReportItemWrapper> RequirementList
        {
            get { return _requirementList; }
        }
    }
}