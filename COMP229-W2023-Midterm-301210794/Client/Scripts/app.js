
"use strict";
/*app.js-Ze Feng-301210794-midterm-my book list*/

(function () {
    function confirmDelete() {
        $("a.delete").on("click", function (event) {
            if (!confirm("Are you sure?")) {
                event.preventDefault();
                location.href = '/books';
            }
        });
    }
    function Start() {
        console.log("App Started");
        confirmDelete();
    }
    window.addEventListener("load", Start);
})();
//# sourceMappingURL=app.js.map