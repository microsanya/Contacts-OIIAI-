﻿using System;
using System.Windows.Input;

/// <summary>
/// Реализует интерфейс для привязки команд в WPF
/// </summary>
public class RelayCommand : ICommand
{
    /// <summary>
    /// Конструктор команды.
    /// </summary>
    /// <param name="execute">Метод, который будет выполнен при вызове команды.</param>
    /// <param name="canExecute">Метод, который проверяет, можно ли выполнить команду.</param>
    public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
    {
        if (execute == null)
        {
            throw new ArgumentNullException(nameof(execute));
        }

        _execute = execute;
        _canExecute = canExecute;
    }

    /// <summary>
    /// Делегат, который представляет метод, выполняющийся при вызове команды.
    /// </summary>
    private readonly Action<object> _execute;

    /// <summary>
    /// Делегат, который представляет метод, проверяющий, может ли команда быть выполнена.
    /// </summary>
    private readonly Predicate<object> _canExecute;

    /// <summary>
    /// Событие, которое вызывается при изменении возможности выполнения команды.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add
        {
            CommandManager.RequerySuggested += value;
        }

        remove
        {
            CommandManager.RequerySuggested -= value;
        }
    }

    /// <summary>
    /// Проверяет, можно ли выполнить команду.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns>True, если команду можно выполнить, иначе False.</returns>
    public bool CanExecute(object parameter)
    {
        return _canExecute == null || _canExecute(parameter);
    }

    /// <summary>
    /// Выполняет команду.
    /// </summary>
    /// <param name="parameter">Параметр команды.</param>
    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}