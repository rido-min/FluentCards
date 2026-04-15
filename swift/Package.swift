// swift-tools-version: 5.9
import PackageDescription

let package = Package(
    name: "FluentCards",
    platforms: [
        .macOS(.v12)
    ],
    products: [
        .library(name: "FluentCards", targets: ["FluentCards"]),
        .executable(name: "Samples", targets: ["Samples"]),
    ],
    targets: [
        .target(
            name: "FluentCards",
            path: "Sources/FluentCards"
        ),
        .executableTarget(
            name: "Samples",
            dependencies: ["FluentCards"],
            path: "Samples"
        ),
        .testTarget(
            name: "FluentCardsTests",
            dependencies: ["FluentCards"],
            path: "Tests/FluentCardsTests"
        ),
    ]
)
