namespace Designer.Controllers.App.Commands {
	public interface ICommand {
		void Execute();
		void Undo();
		void Redo();
	}
}