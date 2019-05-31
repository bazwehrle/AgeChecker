/*
  
    site.js
    Implements access log table filtering
   
 */
var Filtering = (function (Filtering, $, window, undefined) {

    /*
     * jQuery DOM element selector helper
     * @param {string} selector - jQuery selector string
     * @returns {object} - containing input selector and collection method call to return the most up to date dom query results
     */
    $.extend({
        jqel: function (selector) {
            return {
                "selector": selector,
                "collection": function () { return $(selector); }
            };
        }
    });



    /**
     * jQuery references to page elements
     */
    var _elements = {
        $trAccessLogs: $.jqel(".table tbody tr"),
        $tdColsToFilter: $.jqel(".text-filter"),
        $tbFilter: $.jqel("#tb_filter")
    };


    /**
     * Apply filter text to table data
     */
    var _doFilter = function () {
        let me = this,
            $me = $(me),
            filterText = $me.val().toLowerCase();

        _elements.$trAccessLogs.collection().each(function () {
            let $row = $(this),
                textFound = ($(_elements.$tdColsToFilter.selector, this).text().toLowerCase().indexOf(filterText) > -1);

            if (textFound) {
                $row.show();
            }
            else {
                $row.hide();
            }
        });
    };


   /**
    * Delegated event handler bindings
    */
    (function () {
        _elements.$tbFilter.collection().on("keyup", _doFilter);
    }());
    

}(Filtering || {}, jQuery, window, undefined));