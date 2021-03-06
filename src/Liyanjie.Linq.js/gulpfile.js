﻿const gulp = require('gulp');
const plugins = require('gulp-load-plugins')();
const
    rename = plugins.rename,
    minifyJs = plugins.uglify;

function dist(cp) {
    gulp.src('./bundles/liyanjie.linq.umd.js')
        .pipe(rename('liyanjie.linq.js'))
        .pipe(gulp.dest('./dist'))
        .pipe(rename({ suffix: '.min' }))
        .pipe(minifyJs())
        .pipe(gulp.dest('./dist'));
    cp();
}

exports.dist = dist;