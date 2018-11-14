function showTravelAgencyMenu() {
    $('#travelagencyMenu').show();
}
function hideTravelAgencyMenu() {
    $('#travelagencyMenu').hide();
}
function registerSummernote(element, placeholder, max, callbackMax, height) {
    if (height == null || height == undefined) {
        height = 300;
    }
    max = 10000;
    $(element).summernote({
        //toolbar: [
        //    // [groupName, [list of button]]
        //    ['style', ['bold', 'italic', 'underline', 'clear']],
        //    ['font', ['strikethrough', 'superscript', 'subscript']],
        //    ['fontsize', ['fontsize']],
        //    ['color', ['color']],
        //    ['para', ['ul', 'ol', 'paragraph']],
        //    ['height', ['height']],
            
        //],
        dialogsInBody: true,
        placeholder,
        tabsize: 2,
        callbacks: {
            onKeydown: function (e) {
                var t = e.currentTarget.innerText;
                if (t.trim().length >= max) {
                    //delete key
                    if (e.keyCode != 8)
                        e.preventDefault();
                    // add other keys ...
                }
            },
            onKeyup: function (e) {
                var t = e.currentTarget.innerText;
                if (typeof callbackMax == 'function') {
                    callbackMax(max - t.trim().length);
                }
            },
            onPaste: function (e) {
                var t = e.currentTarget.innerText;
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
                e.preventDefault();
                var all = t + bufferText;
                document.execCommand('insertText', false, all.trim().substring(0, 400));
                if (typeof callbackMax == 'function') {
                    callbackMax(max - t.length);
                }
            }
        }
    });
}