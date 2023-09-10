using UniRx;

public interface SkinModelInterface 
{
    IReadOnlyReactiveProperty<int> selectedNum { get; }
    public IReadOnlyReactiveCollection<int> activatedNumList { get; }

    void Load();

    void Save();

    public void ChangeSelectedNum(int num);

    public void AddList(int num);

    void RandomActivate();
}
