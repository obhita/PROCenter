(function($) {
    $.fn.handleCheckAll = function() {
        $(this).each(function() {
            var $checkAllDiv = $(this);
            var checkAllHtml = '<label tabindex="0"><input type="checkbox" class="checkall"/><b>Check All</b></label>';
            $checkAllDiv.html(checkAllHtml + $checkAllDiv.html());

            var checllAllInput = $checkAllDiv.find("input.checkall");
            checllAllInput.prop('checked', isAllChecked($checkAllDiv));

            checllAllInput.on('click',
                function() {
                    var checked = $(this).prop('checked');
                    $checkAllDiv.find('input[class!="checkall"]').prop('checked', checked);
                });

            $checkAllDiv.find('input[class!="checkall"]').click(function() {
                checllAllInput.prop('checked', isAllChecked($checkAllDiv));
            });
        });

        function isAllChecked($checkAllDiv) {
            var allChecked = true;
            $checkAllDiv.find('input[class!="checkall"]').each(function() {
                allChecked = allChecked & $(this).prop('checked');
            });
            return allChecked;
        }
    };
})(jQuery);