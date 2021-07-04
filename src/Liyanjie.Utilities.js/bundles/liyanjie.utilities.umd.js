(function (global, factory) {
    typeof exports === 'object' && typeof module !== 'undefined' ? factory(exports) :
    typeof define === 'function' && define.amd ? define(['exports'], factory) :
    (global = global || self, factory((global.liyanjie = global.liyanjie || {}, global.liyanjie.utilities = {})));
}(this, (function (exports) { 'use strict';

    Date.prototype.format = function (format, weekDisplay) {
        if (weekDisplay === void 0) { weekDisplay = {}; }
        var o = {
            "M{1,2}": this.getMonth() + 1,
            "d{1,2}": this.getDate(),
            "h{1,2}": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12,
            "H{1,2}": this.getHours(),
            "m{1,2}": this.getMinutes(),
            "s{1,2}": this.getSeconds(),
        };
        var w = {
            "0": weekDisplay.sun || "星期日",
            "1": weekDisplay.mon || "星期一",
            "2": weekDisplay.tue || "星期二",
            "3": weekDisplay.wed || "星期三",
            "4": weekDisplay.thu || "星期四",
            "5": weekDisplay.fri || "星期五",
            "6": weekDisplay.sat || "星期六"
        };
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, this.getFullYear().toString().substr(4 - RegExp.$1.length));
        }
        if (/(d{3,4})/.test(format)) {
            format = format.replace(RegExp.$1, w[this.getDay().toString()]);
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                var value = RegExp.$1.length === 1
                    ? (o[k])
                    : ("00" + o[k]).substr(o[k].toString().length);
                format = format.replace(RegExp.$1, value);
            }
        }
        if (/(f{1,3})/.test(format)) {
            format = format.replace(RegExp.$1, this.getMilliseconds().toString().substr(3 - RegExp.$1.length));
        }
        return format;
    };
    Date.prototype.addMillionSeconds = function (millionSeconds) {
        var date = new Date(this.getTime());
        date.setMilliseconds(date.getMilliseconds() + millionSeconds);
        return date;
    };
    Date.prototype.addSeconds = function (seconds) {
        var date = new Date(this.getTime());
        date.setSeconds(this.getSeconds() + seconds);
        return date;
    };
    Date.prototype.addMinutes = function (minutes) {
        var date = new Date(this.getTime());
        date.setMinutes(date.getMinutes() + minutes);
        return date;
    };
    Date.prototype.addHours = function (hours) {
        var date = new Date(this.getTime());
        date.setHours(date.getHours() + hours);
        return date;
    };
    Date.prototype.addDays = function (days) {
        var date = new Date(this.getTime());
        date.setDate(date.getDate() + days);
        return date;
    };
    Date.prototype.addMonths = function (months) {
        var date = new Date(this.getTime());
        date.setMonth(date.getMonth() + months);
        return date;
    };
    Date.prototype.addYears = function (years) {
        var date = new Date(this.getTime());
        date.setFullYear(date.getFullYear() + years);
        return date;
    };

    function __toCnNumber(number, uppercase) {
        switch (number) {
            case 0:
                return uppercase ? '零' : '〇';
            case 1:
                return uppercase ? '壹' : '一';
            case 2:
                return uppercase ? '贰' : '二';
            case 3:
                return uppercase ? '叁' : '三';
            case 4:
                return uppercase ? '肆' : '四';
            case 5:
                return uppercase ? '伍' : '五';
            case 6:
                return uppercase ? '陆' : '六';
            case 7:
                return uppercase ? '柒' : '七';
            case 8:
                return uppercase ? '捌' : '八';
            case 9:
                return uppercase ? '玖' : '九';
            case 10:
                return uppercase ? '拾' : '十';
            case 100:
                return uppercase ? '佰' : '百';
            case 1000:
                return uppercase ? '仟' : '千';
            case 10000:
                return '万';
            case 100000000:
                return '亿';
            case 1000000000000:
                return '兆';
            case 10000000000000000:
                return '京';
        }
    }
    Number.prototype.plus = function (arg) {
        var r1, r2, m;
        try {
            r1 = this.toString().split('.')[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg.toString().split('.')[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        m = Math.pow(10, Math.max(r1, r2));
        return (this * m + arg * m) / m;
    };
    Number.prototype.minus = function (arg) {
        var r1, r2, m, n;
        try {
            r1 = this.toString().split('.')[1].length;
        }
        catch (e) {
            r1 = 0;
        }
        try {
            r2 = arg.toString().split('.')[1].length;
        }
        catch (e) {
            r2 = 0;
        }
        m = Math.pow(10, Math.max(r1, r2));
        n = (r1 >= r2) ? r1 : r2;
        return Number(((this * m - arg * m) / m).toFixed(n));
    };
    Number.prototype.multipy = function (arg) {
        var m = 0, s1 = this.toString(), s2 = arg.toString();
        try {
            m += s1.split('.')[1].length;
        }
        catch (e) { }
        try {
            m += s2.split('.')[1].length;
        }
        catch (e) { }
        return Number(s1.replace('.', '')) * Number(s2.replace('.', '')) / Math.pow(10, m);
    };
    Number.prototype.divide = function (arg) {
        var t1 = 0, t2 = 0, r1, r2;
        try {
            t1 = this.toString().split('.')[1].length;
        }
        catch (e) { }
        try {
            t2 = arg.toString().split('.')[1].length;
        }
        catch (e) { }
        r1 = Number(this.toString().replace('.', ''));
        r2 = Number(arg.toString().replace('.', ''));
        return (r1 / r2) * Math.pow(10, t2 - t1);
    };
    Number.prototype.toCnNumber = function (uppercase) {
        if (uppercase === void 0) { uppercase = false; }
        var i = parseInt(this.toString());
        if (i >= 10000000000000000)
            i = 10000000000000000;
        if (i >= 1000000000000)
            i = 1000000000000;
        if (i >= 100000000)
            i = 100000000;
        else if (i >= 10000)
            i = 10000;
        else if (i >= 1000)
            i = 1000;
        else if (i >= 100)
            i = 100;
        else if (i >= 10)
            i = 10;
        else if (i < 0)
            i = 0;
        return __toCnNumber(i, uppercase);
    };
    Number.prototype.toCn = function (numberType) {
        if (numberType === void 0) { numberType = 'Normal'; }
        numberType = numberType.toLowerCase();
        var number = this.toString();
        var s = '';
        if (number[0] == '-') {
            s += '负';
            number = number.substr(1);
        }
        var unit = function (i) {
            switch (i) {
                case 0:
                    return '角';
                case 1:
                    return '分';
                case 2:
                    return '厘';
                default:
                    return '';
            }
        }, processLong = function (number, uppercase) {
            var sb = '';
            var zeroFlag = false; //输出0
            if (number > 1) {
                var l = number;
                for (var i = 4; i >= 0; i--) {
                    var level = Math.pow(10000, i);
                    if (number >= level) {
                        l = number % level;
                        number = parseInt((number / level).toString());
                        if (number > 19) {
                            var j = 1000;
                            while (number % (j * 10) >= 1) {
                                var tmp = parseInt((number / j).toString());
                                if (tmp != 0) {
                                    sb += __toCnNumber(tmp, uppercase);
                                    if (j > 1)
                                        sb += __toCnNumber(j, uppercase);
                                    zeroFlag = true;
                                }
                                else if (zeroFlag) {
                                    sb += __toCnNumber(0, uppercase);
                                    zeroFlag = false;
                                }
                                if (j == 1)
                                    break;
                                number %= j;
                                j = j.divide(10);
                            }
                        }
                        else if (number >= 10) {
                            sb += __toCnNumber(10, uppercase);
                            if (number % 10 > 0) {
                                sb += __toCnNumber(number % 10, uppercase);
                                zeroFlag = true;
                            }
                        }
                        else
                            sb += __toCnNumber(number, uppercase);
                        if (level > 1)
                            sb += __toCnNumber(level, uppercase);
                    }
                    number = l;
                }
            }
            else
                sb += __toCnNumber(number, uppercase);
            return sb;
        }, processDecimal = function (decimal) {
            var sb = '';
            var zeroFlag = false; //输出0
            for (var i = 0; i < decimal.length; i++) {
                var d = parseInt(decimal[i]);
                if (d > 0) {
                    sb += __toCnNumber(d, true);
                    sb += unit(i);
                    zeroFlag = true;
                }
                else if (decimal.length > i + 1 && zeroFlag) {
                    sb += __toCnNumber(0, true);
                    zeroFlag = false;
                }
            }
            return sb;
        };
        var int = parseInt(number);
        switch (numberType) {
            case 'normal':
                s += processLong(int, false);
                break;
            case 'normalupper':
                s += processLong(int, true);
                break;
            case 'direct':
                {
                    var s_ = int.toString();
                    for (var i = 0; i < s_.length; i++) {
                        s += __toCnNumber(parseInt(s_[i]), false);
                    }
                }
                break;
            case 'directupper':
                {
                    var s_ = int.toString();
                    for (var i = 0; i < s_.length; i++) {
                        s += __toCnNumber(parseInt(s_[i]), true);
                    }
                }
                break;
            case 'currency':
                s += processLong(int, true);
                break;
        }
        var dec, index = number.indexOf('.');
        if (index > 0)
            dec = number.substr(index + 1);
        if (dec) {
            switch (numberType) {
                case 'normal':
                case 'normalupper':
                case 'direct':
                case 'directupper':
                    s += '点';
                    break;
                case 'currency':
                    s += '圆';
                    break;
            }
            switch (numberType) {
                case 'normal':
                    for (var i = 0; i < dec.length; i++) {
                        s += __toCnNumber(parseInt(dec[i]), false);
                    }
                    break;
                case 'normalupper':
                    for (var i = 0; i < dec.length; i++) {
                        s += __toCnNumber(parseInt(dec[i]), true);
                    }
                    break;
                case 'direct':
                    for (var i = 0; i < dec.length; i++) {
                        s += __toCnNumber(parseInt(dec[i]), false);
                    }
                    break;
                case 'directupper':
                    for (var i = 0; i < dec.length; i++) {
                        s += __toCnNumber(parseInt(dec[i]), true);
                    }
                    break;
                case 'currency':
                    s += processDecimal(dec);
                    break;
            }
        }
        else if (numberType == 'currency')
            s += '圆整';
        return s;
    };

    /**
    * Guid
    */
    var Guid = /** @class */ (function () {
        function Guid(input) {
            this.array = [];
            /**
             * Guid对象的标记
             */
            this.isGuid = true;
            if (input && typeof (input) === 'string') {
                input = input.replace(/\{|\(|\)|\}|\-/g, '');
                input = input.toLowerCase();
                if (input.length === 32 && input.search(/[^0-9,a-f]/i) < 0) {
                    for (var i = 0; i < input.length; i++) {
                        this.array.push(input[i]);
                    }
                }
            }
            if (this.array.length === 0) {
                for (var i = 0; i < 32; i++) {
                    this.array.push('0');
                }
            }
        }
        /**
         * 返回一个值，该值指示 Guid 的两个实例是否表示同一个值
         * @param other
         * @returns
         */
        Guid.prototype.equals = function (other) {
            if (other && other.isGuid)
                return this.toString() == other.toString();
            return false;
        };
        /**
         * 返回 Guid 类的此实例值的 String 表示形式。
         * 根据所提供的格式说明符，返回此 Guid 实例值的 String 表示形式。
         * N  32 位： xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
         * D  由连字符分隔的 32 位数字 xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
         * B  括在大括号中、由连字符分隔的 32 位数字：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
         * P  括在圆括号中、由连字符分隔的 32 位数字：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
         * @param format
         * @returns
         */
        Guid.prototype.format = function (format) {
            if (format === void 0) { format = 'N'; }
            if (format)
                switch (format) {
                    case 'N':
                        return this.array.toString().replace(/,/g, '');
                    case 'D':
                        return (this.array.slice(0, 8) + "-" + this.array.slice(8, 12) + "-" + this.array.slice(12, 16) + "-" + this.array.slice(16, 20) + "-" + this.array.slice(20, 32)).replace(/,/g, '');
                    case 'B':
                        return "{" + this.format('D') + "}";
                    case 'P':
                        return "(" + this.format('D') + ")";
                    default:
                        throw new Error("Parameter “format” must be one of N|D|B|P]");
                }
            else
                return this.format('D');
        };
        /**
         * 初始化 Guid 类的一个新实例
         */
        Guid.newGuid = function () {
            var string = '';
            for (var i = 0; i < 32; i++) {
                string += Math.floor(Math.random() * 16.0).toString(16);
            }
            return new Guid(string);
        };
        /**
         * Guid 类的默认实例，其值保证均为零
         */
        Guid.empty = new Guid();
        return Guid;
    }());

    exports.Guid = Guid;

    Object.defineProperty(exports, '__esModule', { value: true });

})));
