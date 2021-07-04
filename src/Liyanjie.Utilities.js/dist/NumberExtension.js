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
        default:
            break;
    }
}
;
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
                break;
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
        default:
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
            default:
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
            default:
                break;
        }
    }
    else if (numberType == 'currency')
        s += '圆整';
    return s;
};
//# sourceMappingURL=NumberExtension.js.map