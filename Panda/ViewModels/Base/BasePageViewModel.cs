using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Panda.ViewModels.Base;

// Inherits from ObservableObject which uses rosyln to implement INotifyPropertyChanged for binding and no boilerplate
// Also used as a base for my ViewLocator
public abstract class BasePageViewModel : ObservableObject;