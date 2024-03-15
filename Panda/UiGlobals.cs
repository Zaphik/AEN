using System.Collections.ObjectModel;
using Panda.Server;

namespace Panda;

// Why create a separate delegate for this? Why not just use a regular Action<List<string?>> like you did in regViewModel I hear you ask. Idrk, it was just too late to change it when I realised
public delegate void QnA(string QUESTION, string ANSWER);

// Ok again, sure, this class is called UiGlobals, and sure, there is 2 static variables here. BUT, you'd agree that it makes sense right?
public abstract class UiGlobals
{
    // Why have a separate UserID instead of passing it around through a delegate like you've been doing previously? So basically, the delegate was before the UserID was fully solidified,
    // so b4 the user logged in or registered, bc stuff could go wrong. After they've actually logged in, then we save it as a global 
    public static int? UserID { get; set; }
    
    
    // DI woulda worked but this is state ygm
    public static ObservableCollection<Settings> settingsList { get; set; }
}