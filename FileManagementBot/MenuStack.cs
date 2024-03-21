using Telegram.Bot.Types.ReplyMarkups;
namespace FileManagementBot;

/// <summary>
/// Represents a stack for managing inline keyboard markups in Telegram bot menus.
/// </summary>
public class MenuStack
{
    // Singleton instance of the MenuStack class.
    private static MenuStack s_instance;
    
    // The stack containing inline keyboard markups.
    private Stack<InlineKeyboardMarkup> _menuStack = new Stack<InlineKeyboardMarkup>();
    
    /// <summary>
    /// Gets the singleton instance of the MenuStack class.
    /// </summary>
    /// <returns>The singleton instance of the MenuStack class.</returns>
    internal static MenuStack GetInstance()
    {
        if (s_instance == null)
            s_instance = new MenuStack();

        return s_instance;
    }

    /// <summary>
    /// Pushes an inline keyboard markup onto the menu stack.
    /// </summary>
    /// <param name="inlineKeyboard">The inline keyboard markup to push onto the stack.</param>
    internal async Task PushInlineKeyboardMarkupInMenuStack(InlineKeyboardMarkup inlineKeyboard)
    {
        _menuStack.Push(inlineKeyboard);
    }

    /// <summary>
    /// Pops an inline keyboard markup from the menu stack.
    /// </summary>
    /// <returns>The inline keyboard markup popped from the stack, or null if the stack is empty.</returns>
    internal async Task<InlineKeyboardMarkup> PopInlineKeyboardMarkupFromMenuStack()
    {
        if (_menuStack.Count > 1)
        {
            _menuStack.Pop();
            return _menuStack.Peek();
        }

        return null!;
    }
}