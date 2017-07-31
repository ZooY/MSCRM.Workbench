using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;


namespace PZone.ViewModels
{
    // ReSharper disable once InconsistentNaming
    public class BuildJS : ViewModel
    {
        private EntityMetadata _entity;


        public ObservableCollection<EntityMetadata> Entities { get; } = new ObservableCollection<EntityMetadata>();


        public EntityMetadata Entity
        {
            get { return _entity; }
            set { SetProperty(ref _entity, value); }
        }


        public BuildJS()
        {
            PropertyChanged += (s, e) =>
            {
                //if(e.PropertyName == "Entity")

            };
            
            var entities = ((RetrieveAllEntitiesResponse)App.Service.Execute(new RetrieveAllEntitiesRequest { EntityFilters = EntityFilters.Entity })).EntityMetadata.OrderBy(e => e.LogicalName);
            foreach (var entity in entities)
                Entities.Add(entity);
        }
    }
}