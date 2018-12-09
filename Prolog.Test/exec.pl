:- ensure_loaded([db]).

loves_data(X, Y) :- loves(X, Y), write(X), write(':'), write(Y), nl.

list_preferences(X, Y) :- findall(Prefer, loves_data(X, Prefer), Y).