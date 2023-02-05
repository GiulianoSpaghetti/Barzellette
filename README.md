# Barzellette

Un visualizzatore privato di fortune.

# Come funziona

Si connette ad un database access a 64 bit appositamente formattato e visualizza in formato HTML il contenuto dello stesso.
Il database deve avere una tabella chiamata Barzellette contenente due campi: ID e Testo, che dovrebbe essere in formato RTF, con all'interno una serie di frasi ad effetto.

La path del database va passata come parametro.

Il programma si accorge se il database non esiste o se non è  opportunamente programmato.

# Bibliografia

https://learn.microsoft.com/en-us/dotnet/api/system.string.replace?view=net-7.0

https://stackoverflow.com/questions/3683450/handling-the-window-closing-event-with-wpf-mvvm-light-toolkit

https://stackoverflow.com/questions/2820357/how-do-i-exit-a-wpf-application-programmatically

https://sa.ndeep.me/post/how-to-create-smart-wpf-command-line-arguments

https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.canceleventhandler?view=net-7.0
