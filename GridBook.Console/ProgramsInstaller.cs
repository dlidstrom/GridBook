namespace GridBook.Console
{
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;

	public class ProgramsInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(AllTypes
				.FromThisAssembly()
				.BasedOn<IProgram>()
				.Configure(c => c.LifeStyle.Transient.Named(c.Implementation.Name))
				.WithService.Base());
		}
	}
}
