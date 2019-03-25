import typescript from "rollup-plugin-typescript";

let config = {
    input: "./src/index.ts",
    output: {
        file: "bundles/liyanjie.linq.umd.js",
        name: "liyanjie.linq",
        format: "umd"
    },
    plugins: [
        typescript()
    ]
};
export default config;