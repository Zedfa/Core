Call this to setup bootstrapper first,

ContainerBootstrapper.BootstrapStructureMap<TypeWhereYourEntitiesAreForNHibernate>("myConnectionStringName");

You can then use ObjectFactory.GetInstance<IRepository>() to get repository back.

Unit Of Work is not a static class but can be called with static methods.