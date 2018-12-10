:- ensure_loaded([db]).

likes_data(X, Y) :- likes(X, Y), write(Y), nl.
list_preferences(X, Y) :- findall(Prefer, likes_data(X, Prefer), Y).

recommended_data(X, Y) :- recommended(X, Y), write(Y), nl.
list_recommended(X, Y) :- findall(Recommended, recommended_data(X, Recommended), Y).