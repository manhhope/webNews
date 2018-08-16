/**
* jQuery jPagging v1.0
* Copyright 2013 Ngô Thanh Hải
*/
; (function ($, window, document, undefined) {

    var name = "jPagging",
      instance = null,
      defaults = {
          container: "",
          titlePage: "",
          previous: "← Trước",
          next: "Tiếp theo →",
          links: "",
          currentNumPage: 1,
          pageSize: 25,
          totalRecord: 1,
          totalPages: 1,

          numRange: 5,
          startRange: false,
          endRange: false
      };

    function Plugin(element, options) {
        this.options = $.extend({}, defaults, options);

        this._container = $(this.options.container);
        if (!this._container.length) return;

        this.options.totalPages = Math.ceil(this.options.totalRecord / this.options.pageSize);

        if (this.options.currentNumPage - this.options.numRange > 1)
        { this.options.startRange = true; }

        if (this.options.currentNumPage + this.options.numRange < this.options.totalPages)
        { this.options.endRange = true; }

        this.init();
    }

    Plugin.prototype = {

        constructor: Plugin,

        init: function () {
            this.setNav();
        },
        setNav: function () {
            var navhtml = this.writeNav();
            this._container.html(navhtml);
        },
        writeNav: function () {
            var i = 1, navhtml, checkDraw = false;

            //previous
            if (this.options.currentNumPage <= 1) { navhtml = "<li class='first'><span>" + this.options.previous + "</span></li>"; }
            else { navhtml = "<li class='first'><a href='" + this.options.links + (this.options.currentNumPage - 1) + "' class='jp-previous jp-item' title='"+this.options.titlePage+" - Trang "+(this.options.currentNumPage - 1)+"'>" + this.options.previous + "</a></li> "; }

            for (; i <= this.options.totalPages; i++) {
                checkDraw = false;

                //first
                if (i === 1 && this.options.currentNumPage - 1 > 1) {
                    navhtml += this.writePageItem(i, ""+this.options.titlePage+" - Trang " + i + "");
                }

                //skip
                if (i === 1 && this.options.startRange === true) navhtml += "<li><span>...</span></li>";

                //range
                if ((i > 1 || this.options.currentNumPage < 3) && ((this.options.currentNumPage - i) <= this.options.numRange) && i <= (this.options.currentNumPage + this.options.numRange)) {
                    navhtml += this.writePageItem(i, ""+this.options.titlePage+" - Trang " + i + "");

                    checkDraw = true;
                }

                //skip
                if (i === this.options.totalPages && this.options.endRange === true) navhtml += "<li><span>...</span></li>";

                //last
                if (checkDraw ==  false && i === this.options.totalPages && this.options.currentNumPage < this.options.totalPages - 1) {
                    navhtml += this.writePageItem(i, ""+this.options.titlePage+" - Trang " + i + "");
                }
            }

            //next
            if (this.options.currentNumPage >= this.options.totalPages) { navhtml += "<li class='last'><span>" + this.options.next + "</span></li>"; }
            else { navhtml += "<li class='last'><a href='" + this.options.links + (this.options.currentNumPage + 1) + "' class='jp-next jp-item' title='"+this.options.titlePage+" - Trang "+(this.options.currentNumPage + 1)+"'>" + this.options.next + "</a></li> "; }

            return navhtml;
        },

        writePageItem: function (pageNumber, pageTitle, pageClass) {
            if (pageNumber == this.options.currentNumPage) {
                return "<li class='active'><span>" + pageNumber + "</span></li>";
            }
            else {
                return "<li><a href='" + this.options.links + pageNumber + "' class='jp-" + pageClass + " jp-item' title='" + pageTitle + "'>" + pageNumber + "</a></li>";
            }
        }
    };


    $.fn[name] = function (arg) {
        var type = $.type(arg);

        if (type === "object") {
            if (this.length && !$.data(this, name)) {
                instance = new Plugin(this, arg);
                this.each(function () {
                    $.data(this, name, instance);
                });
            }
            return this;
        }
    };

})(jQuery, window, document);
